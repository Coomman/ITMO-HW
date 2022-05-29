@file:Suppress("PropertyName")

import java.util.concurrent.atomic.AtomicReference

class Solution(private val _env: Environment) : Lock<Solution.Node>
{
    class Node
    {
        val Locked = AtomicReference(true)
        val Next = AtomicReference<Node?>(null)

        val WaitingThread: Thread = Thread.currentThread() // запоминаем поток, которые создал узел
    }

    private val _tail = AtomicReference<Node?>(null)

    override fun lock(): Node
    {
        val node = Node()
        val prev = _tail.getAndSet(node)

        if (prev != null)
        {
            prev.Next.value = node

            while (node.Locked.value)
            {
                _env.park()
            }
        }

        return node
    }

    override fun unlock(node: Node)
    {
        var next = node.Next.value

        if (next == null)
        {
            if (_tail.compareAndSet(node, null)) return

            while (next == null)
            {
                next = node.Next.value
            }
        }

        next.Locked.value = false
        _env.unpark(next.WaitingThread)
    }
}