package msqueue;

import kotlinx.atomicfu.AtomicRef;

public class MSQueue implements Queue
{
    private static class Node {
        final int X;
        final AtomicRef<Node> Next = new AtomicRef<>(null);

        Node(int x) {
            this.X = x;
        }
    }

    private final AtomicRef<Node> _head;
    private final AtomicRef<Node> _tail;

    public MSQueue()
    {
        Node dummy = new Node(0);

        _head = new AtomicRef<>(dummy);
        _tail = new AtomicRef<>(dummy);
    }

    @Override
    public void enqueue(int x)
    {
        Node newTail = new Node(x);

        while(true)
        {
            Node curTail = _tail.getValue();

            if (curTail.Next.compareAndSet(null, newTail))
            {
                _tail.compareAndSet(curTail, newTail);
                return;
            }

            _tail.compareAndSet(curTail, curTail.Next.getValue());
        }
    }

    @Override
    public int dequeue()
    {
        Node curHead;
        Node newHead;

        do
        {
            curHead = _head.getValue();
            newHead = curHead.Next.getValue();

            if (newHead == null) //only dummy left
                return Integer.MIN_VALUE;
        }
        while(!_head.compareAndSet(curHead, newHead));

        return newHead.X;
    }

    @Override
    public int peek()
    {
        Node dummyHead = _head.getValue();
        Node trueHead = dummyHead.Next.getValue();

        return trueHead == null
                ? Integer.MIN_VALUE
                : trueHead.X;
    }
}