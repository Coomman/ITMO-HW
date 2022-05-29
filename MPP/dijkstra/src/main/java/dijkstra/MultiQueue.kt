package dijkstra

import kotlinx.atomicfu.locks.ReentrantLock
import java.util.*
import kotlin.Comparator
import kotlin.random.Random

class MultiQueue<T>(threadsCount: Int, comparator: Comparator<T>)
{
    private val POP_TRIES = 3

    private val _comparator = comparator

    private val _locks = Array(threadsCount) {ReentrantLock()}
    private val _buckets = Array(threadsCount) { PriorityQueue(comparator) }

    fun push(el: T)
    {
        var bucketIndex: Int

        do
        {
            bucketIndex = getRandomBucketIndex()
        }
        while (!tryLock(bucketIndex))

        _buckets[bucketIndex].add(el)

        unlock(bucketIndex)
    }

    fun pop(): T?
    {
        var firstBucketIndex: Int
        var secondBucketIndex: Int

        var res: Pair<Boolean, T?> = Pair(false, null)

        var curTries= 0
        do
        {
            if (++curTries > POP_TRIES)
                return null

            firstBucketIndex = getRandomBucketIndex()
            secondBucketIndex = getRandomBucketIndex()

            if (firstBucketIndex == secondBucketIndex)
                continue

            if (firstBucketIndex > secondBucketIndex)
            {
                firstBucketIndex = secondBucketIndex.also { secondBucketIndex = firstBucketIndex }
            }

            if (!tryLock(firstBucketIndex))
                continue

            if (!tryLock(secondBucketIndex))
            {
                unlock(firstBucketIndex)
                continue
            }

            res = popCore(firstBucketIndex, secondBucketIndex)
        }
        while(!res.first)

        unlock(firstBucketIndex, secondBucketIndex)

        return res.second!!
    }
    private fun popCore(firstBucketIndex: Int, secondBucketIndex: Int): Pair<Boolean, T?>
    {
        val firstBucket = _buckets[firstBucketIndex]
        val secondBucket = _buckets[secondBucketIndex]

        if (firstBucket.isEmpty())
        {
            if (secondBucket.isEmpty())
            {
                unlock(firstBucketIndex, secondBucketIndex)

                return Pair(false, null)
            }

            return Pair(true, secondBucket.poll())
        }

        if (secondBucket.isEmpty())
            return Pair(true, firstBucket.poll())

        return popBothNotEmpty(firstBucket, secondBucket)
    }
    private fun popBothNotEmpty(firstBucket: PriorityQueue<T>, secondBucket: PriorityQueue<T>): Pair<Boolean, T?>
    {
        val firstRes = firstBucket.peek()
        val secondRes = secondBucket.peek()

        val finalRes: T

        if (_comparator.compare(firstRes, secondRes) == 0)
        {
            if (firstBucket.size > secondBucket.size)
            {
                finalRes = firstRes
                firstBucket.poll()
            }
            else
            {
                finalRes = secondRes
                secondBucket.poll()
            }
        }
        else
        {
            if (_comparator.compare(firstRes, secondRes) < 0)
            {
                finalRes = firstRes
                firstBucket.poll()
            }
            else
            {
                finalRes = secondRes
                secondBucket.poll()
            }
        }

        return Pair(true, finalRes)
    }

    private fun getRandomBucketIndex(): Int
    {
        return Random.nextInt(_buckets.size)
    }

    private fun tryLock(index: Int): Boolean
    {
        return _locks[index].tryLock()
    }
    private fun unlock(index: Int)
    {
        _locks[index].unlock()
    }
    private fun unlock(firstIndex: Int, secondIndex: Int)
    {
        _locks[secondIndex].unlock()
        _locks[firstIndex].unlock()
    }
}