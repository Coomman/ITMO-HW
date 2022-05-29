@file:Suppress("PropertyName")

import kotlinx.atomicfu.AtomicRef
import kotlinx.atomicfu.atomic
import kotlin.coroutines.Continuation
import kotlin.coroutines.resume
import kotlin.coroutines.suspendCoroutine

class SynchronousQueueMS<E> : SynchronousQueue<E>
{
    private class Node<E>(element: E?, val Requested: Boolean)
    {
        val Next = atomic<Node<E?>?>(null)
        val Value = atomic(element)
        var Continuation: Continuation<Unit>? = null
    }

    private val _head: AtomicRef<Node<E?>>
    private val _tail: AtomicRef<Node<E?>>

    init
    {
        val dummy = Node<E?>(null, false)
        _head = atomic(dummy)
        _tail = atomic(dummy)
    }

    override suspend fun send(element: E)
    {
        val offer = Node<E?>(element, false)

        while (true)
        {
            val head = _head.value
            val tail = _tail.value

            if (head == tail || !tail.Requested)
            {
                if (!rendezvous(offer, head, tail)) continue
                return
            }

            val nextNode = head.Next.value ?: continue
            if (head != _head.value || tail != _tail.value) continue
            val continuation = nextNode.Continuation ?: continue

            val changed = nextNode.Value.compareAndSet(null, element)
            _head.compareAndSet(head, nextNode)

            if (changed)
            {
                continuation.resume(Unit)
                return
            }
        }
    }

    override suspend fun receive(): E
    {
        val offer = Node<E?>(null, true)

        while (true)
        {
            val head = _head.value
            val tail = _tail.value

            if (head == tail || tail.Requested)
            {
                if (!rendezvous(offer, head, tail)) continue
                return offer.Value.value!!
            }

            val nextNode = head.Next.value ?: continue
            if (head != _head.value || tail != _tail.value) continue
            val continuation = nextNode.Continuation ?: continue
            val element = nextNode.Value.value ?: continue

            val changed = nextNode.Value.compareAndSet(element, null)
            _head.compareAndSet(head, nextNode)

            if (changed)
            {
                continuation.resume(Unit)
                return element
            }
        }
    }

    private suspend fun rendezvous(offer: Node<E?>, head: Node<E?>, tail: Node<E?>): Boolean
    {
        val nextNode = tail.Next.value

        if (tail == _tail.value)
        {
            if (nextNode != null)
            {
                _tail.compareAndSet(tail, nextNode)
            }
            else if (tail.Next.compareAndSet(null, offer))
            {
                _tail.compareAndSet(tail, offer)
                suspendCoroutine<Unit> { cont ->
                    offer.Continuation = cont
                }

                if (offer == _head.value.Next.value)
                {
                    _head.compareAndSet(head, offer)
                }

                return true
            }
        }

        return false
    }
}
