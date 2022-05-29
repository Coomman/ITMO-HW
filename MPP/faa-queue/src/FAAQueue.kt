import kotlinx.atomicfu.*

private val CRASHED = Any() // Marker for the "DONE" slot state; to avoid memory leaks
const val SEGMENT_SIZE = 2 // DO NOT CHANGE, IMPORTANT FOR TESTS

class FAAQueue<T> {
    private val _head: AtomicRef<Segment> // Head pointer, similarly to the Michael-Scott queue (but the first node is _not_ sentinel)
    private val _tail: AtomicRef<Segment> // Tail pointer, similarly to the Michael-Scott queue

    init {
        val firstSegment = Segment()

        _head = atomic(firstSegment)
        _tail = atomic(firstSegment)
    }

    /**
     * Adds the specified element [x] to the queue.
     */
    fun enqueue(x: T)
    {
        while (true)
        {
            val tail = _tail.value
            var nextTail = tail.Next.value

            if (nextTail != null)
            {
                _tail.compareAndSet(tail, nextTail)
                continue
            }

            val enqIdx = tail.EnqIdx
            if (enqIdx < SEGMENT_SIZE)
            {
                if (tail.Data[enqIdx].compareAndSet(null, x)) return

                continue
            }

            nextTail = Segment(x)

            if (tail.Next.compareAndSet(null, nextTail)) return
        }
    }

    /**
     * Retrieves the first element from the queue
     * and returns it; returns `null` if the queue
     * is empty.
     */
    fun dequeue(): T?
    {
        while (true)
        {
            val head = _head.value
            val deqIdx = head.DeqIdx

            if (deqIdx >= SEGMENT_SIZE)
            {
                val nextHead = head.Next.value ?: return null

                _head.compareAndSet(head, nextHead)
                continue
            }

            val res = head.Data[deqIdx].getAndSet(CRASHED) ?: continue

            return res as T
        }
    }

    /**
     * Returns `true` if this queue is empty;
     * `false` otherwise.
     */
    val isEmpty: Boolean get()
    {
        while (true)
        {
            val head = _head.value

            if (head.isEmpty)
            {
                val nextHead = head.Next.value ?: return true

                _head.compareAndSet(head, nextHead)
                continue
            }

            return false
        }
    }
}

