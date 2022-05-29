@file:Suppress("PropertyName")

import kotlinx.atomicfu.AtomicIntArray
import kotlinx.atomicfu.atomic

/**
 * Int-to-Int hash map with open addressing and linear probes.
 */
class IntIntHashMap
{
    private val _core = atomic(Core(INITIAL_CAPACITY))

    /**
     * Returns value for the corresponding key or zero if this key is not present.
     *
     * @param key a positive key.
     * @return value for the corresponding or zero if this key is not present.
     * @throws IllegalArgumentException if key is not positive.
     */
    operator fun get(key: Int): Int
    {
        require(key > 0) { "Key must be positive: $key" }
        while (true)
        {
            val core = _core.value

            val res = core.getInternal(key)
            if (res != NEEDS_REHASH)
                return toValue(res)

            core.rehash()
            _core.compareAndSet(core, core.Next.value!!)
        }
    }

    /**
     * Changes value for the corresponding key and returns old value or zero if key was not present.
     *
     * @param key   a positive key.
     * @param value a positive value.
     * @return old value or zero if this key was not present.
     * @throws IllegalArgumentException if key or value are not positive, or value is equal to
     * [Integer.MAX_VALUE] which is reserved.
     */
    fun put(key: Int, value: Int): Int
    {
        require(key > 0) { "Key must be positive: $key" }
        require(isValue(value)) { "Invalid value: $value" }
        return toValue(putCore(key, value))
    }

    /**
     * Removes value for the corresponding key and returns old value or zero if key was not present.
     *
     * @param key a positive key.
     * @return old value or zero if this key was not present.
     * @throws IllegalArgumentException if key is not positive.
     */
    fun remove(key: Int): Int
    {
        require(key > 0) { "Key must be positive: $key" }
        return toValue(putCore(key, DEL_VALUE))
    }

    private fun putCore(key: Int, value: Int): Int
    {
        while (true)
        {
            val core = _core.value
            val res = core.putInternal(key, value)
            if (res != NEEDS_REHASH) return res

            core.rehash()
            _core.compareAndSet(core, core.Next.value!!)
        }
    }

    private class Core(capacity: Int)
    {
        // Pairs of <key, value> here, the actual size of the map is twice as big.
        private val _size = capacity * 2
        private val _array = AtomicIntArray(_size)
        private val _shift: Int

        val Next = atomic<Core?>(null)

        init
        {
            val mask = capacity - 1
            assert(mask > 0 && mask and capacity == 0) { "Capacity must be power of 2: $capacity" }
            _shift = 32 - Integer.bitCount(mask)
        }

        fun getInternal(key: Int): Int
        {
            var hash = index(key)

            for (probes in 0 until MAX_PROBES)
            {
                val internalValue = _array[hash + 1].value
                if (internalValue == MOVED_VALUE) return NEEDS_REHASH

                val internalKey = _array[hash].value
                if (internalKey == NULL_KEY) return NULL_VALUE
                if (internalKey == key) return unlock(internalValue)

                hash = (hash + 2) % _size
            }

            return NULL_VALUE
        }

        fun putInternal(key: Int, value: Int): Int
        {
            var hash = index(key)

            for (probes in 0 until MAX_PROBES)
            {
                val internalValue: Int = _array[hash + 1].value
                if (isLocked(internalValue)) return NEEDS_REHASH

                val internalKey: Int = _array[hash].value
                if (internalKey == NULL_KEY)
                {
                    if (value == DEL_VALUE) return NULL_VALUE
                    if (setKeyValue(hash, key, value)) return internalValue
                    continue
                }
                if (internalKey == key)
                {
                    if (_array[hash + 1].compareAndSet(internalValue, value)) return internalValue
                    continue
                }

                hash = (hash + 2) % _size
            }

            return NEEDS_REHASH
        }

        fun rehash()
        {
            fun getUnlocked(index: Int): Int
            {
                while (true)
                {
                    val value = _array[index].value
                    if (isLocked(value)) return unlock(value)
                    if (_array[index].compareAndSet(value, lock(value))) return unlock(value)
                }
            }

            Next.compareAndSet(null, Core(_size * 2))

            for (i in 0 until _size step 2)
            {
                if (_array[i + 1].value == MOVED_VALUE) continue

                val value = getUnlocked(i + 1)
                Next.value!!.move(_array[i].value, value)

                _array[i + 1].getAndSet(MOVED_VALUE)
            }
        }

        fun move(key: Int, value: Int)
        {
            var hash = index(key)

            for (probes in 0 until MAX_PROBES)
            {
                val internalKey: Int = _array[hash].value
                if (internalKey == NULL_KEY)
                {
                    if (setKeyValue(hash, key, value)) return
                    continue
                }
                if (internalKey == key) {
                    _array[hash + 1].compareAndSet(NULL_VALUE, value)
                    return
                }

                hash = (hash + 2) % _size
            }

            throw IllegalStateException("Probes are exceeded")
        }

        /**
         * Returns an initial index in map to look for a given key.
         */
        private fun index(key: Int): Int = (key * MAGIC ushr _shift) * 2

        private fun setKeyValue(hash:Int, key: Int, value: Int): Boolean = _array[hash].compareAndSet(NULL_KEY, key) && _array[hash + 1].compareAndSet(NULL_VALUE, value)
    }
}

private const val MAGIC = -0x61c88647 // golden ratio
private const val MOST_LEFT_BIT = 1 shl 31 // int with only most left bit 1
private const val INITIAL_CAPACITY = 2 // !!! DO NOT CHANGE INITIAL CAPACITY !!!
private const val MAX_PROBES = 8 // max number of probes to find an item
private const val NULL_KEY = 0 // missing key (initial value)
private const val NULL_VALUE = 0 // missing value (initial value)
private const val DEL_VALUE = Int.MAX_VALUE // mark for removed value
private const val MOVED_VALUE = Int.MIN_VALUE // mark for moved value
private const val NEEDS_REHASH = -1 // returned by `putInternal` to indicate that rehash is needed

// Checks is the value is in the range of allowed values
private fun isValue(value: Int): Boolean = value in (1 until DEL_VALUE)

// Converts internal value to the public results of the methods
private fun toValue(value: Int): Int = if (isValue(value)) value else 0

private fun lock(value: Int): Int = value or MOST_LEFT_BIT
private fun unlock(value: Int): Int = value and MOST_LEFT_BIT.inv()
private fun isLocked(value: Int): Boolean = value and MOST_LEFT_BIT != 0