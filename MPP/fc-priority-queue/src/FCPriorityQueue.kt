@file:Suppress("PropertyName", "PrivatePropertyName")

import java.util.*
import kotlinx.atomicfu.*
import kotlinx.atomicfu.locks.ReentrantLock
import kotlin.random.Random

class FCPriorityQueue<E : Comparable<E>>
{
    private enum class OperationType { Poll, Peek, Add }

    private class Request<E>(val OpType: OperationType, var Value: E? = null)
    {
        var Done = false
    }

    private val _q = PriorityQueue<E>()

    private val _lock = ReentrantLock()
    private val _requests = atomicArrayOfNulls<Request<E>>(Thread.activeCount() shl 3)

    private val PUT_REQUEST_TRIES = 10

    private fun combine(request: Request<E>): E?
    {
        var requestIndex: Int
        while (true)
        {
            if (_lock.tryLock())
            {
                execute(request)
                processRequests()

                return request.Value
            }

            requestIndex = putRequestAndGetIndex(request)

            if (requestIndex != -1) break
        }

        while (!request.Done)
        {
            if (_lock.tryLock())
            {
                processRequests()

                break
            }
        }

        _requests[requestIndex].value = null

        return request.Value
    }

    private fun processRequests()
    {
        for (i in 0 until _requests.size)
        {
            val request = _requests[i].value ?: continue

            if (!request.Done)
            {
                execute(request)
            }
        }

        _lock.unlock()
    }

    private fun execute(request: Request<E>)
    {
        when (request.OpType)
        {
            OperationType.Poll -> request.Value = _q.poll()
            OperationType.Peek -> request.Value = _q.peek()
            OperationType.Add -> _q.add(request.Value!!)
        }

        request.Done = true
    }

    private fun putRequestAndGetIndex(request: Request<E>): Int
    {
        fun tryPutRequest(index: Int) = _requests[index].compareAndSet(null, request)

        for (i in 0 until PUT_REQUEST_TRIES)
        {
            val index = Random.nextInt(0, _requests.size)

            if (tryPutRequest(index)) return index
            if (index - 1 > -1 && tryPutRequest(index - 1)) return index - 1
            if (index + 1 < _requests.size && tryPutRequest(index + 1)) return index + 1
        }

        return -1
    }

    fun poll(): E? = combine(Request(OperationType.Poll))

    fun peek(): E? = combine(Request(OperationType.Peek))

    fun add(element: E)
    {
        combine(Request(OperationType.Add, element))
    }
}