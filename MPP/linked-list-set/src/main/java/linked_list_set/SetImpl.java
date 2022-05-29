package linked_list_set;

import kotlinx.atomicfu.AtomicRef;
import org.jetbrains.annotations.NotNull;

import java.util.Objects;

public class SetImpl implements Set
{
    private abstract static class NodeBase
    {
        public abstract Node asNode();
        public abstract RemovedNode asRemoved();
    }

    private static class Node extends NodeBase
    {
        final int X;
        final AtomicRef<NodeBase> Next;

        Node(int x, NodeBase next)
        {
            this.X = x;
            this.Next = new AtomicRef<>(next);
        }

        @Override
        public Node asNode()
        {
            return this;
        }

        @Override
        public RemovedNode asRemoved()
        {
            return null;
        }
    }
    private static class RemovedNode extends NodeBase
    {
        final Node RealNext;

        RemovedNode(@NotNull Node next)
        {
            RealNext = next;
        }

        @Override
        public Node asNode()
        {
            return null;
        }

        @Override
        public RemovedNode asRemoved()
        {
            return this;
        }
    }

    private static class Window
    {
        final Node Cur;
        final Node Next;

        public Window(Node cur, Node next)
        {
            this.Cur = cur;
            this.Next = next;
        }
    }
    private static class WindowSearchResult
    {
        final Window Window;
        final AtomicRef<NodeBase> PossibleStart;

        WindowSearchResult(Window window)
        {
            this.Window = window;
            this.PossibleStart = null;
        }

        WindowSearchResult(AtomicRef<NodeBase> possibleStart)
        {
            this.Window = null;
            this.PossibleStart = possibleStart;
        }

        public boolean foundWindow()
        {
            return Window != null;
        }
    }

    private final AtomicRef<NodeBase> _head = new AtomicRef<NodeBase>(new Node(Integer.MIN_VALUE, new Node(Integer.MAX_VALUE, null)));

    /**
     * Returns the {@link Window}, where cur.x < x <= next.x
     */
    private Window findWindow(int x)
    {
        Node cur = _head.getValue().asNode();
        Node next = cur.Next.getValue().asNode();

        while (true)
        {
            WindowSearchResult res = findWindowCore(cur, next, x);

            if (res.foundWindow())
                return res.Window;

            Window newStart = getNewStart(Objects.requireNonNull(res.PossibleStart), x);

            cur = newStart.Cur;
            next = newStart.Next;
        }
    }
    /**
     * Trying to find a window and return it.
     * Otherwise, returns a possible new start point.
     */
    private WindowSearchResult findWindowCore(Node cur, @NotNull Node next, int x)
    {
        AtomicRef<NodeBase> curRef = _head;

        while (true)
        {
            NodeBase nextNext = next.Next.getValue();

            if (nextNext instanceof RemovedNode)
            {
                Node realNext = nextNext.asRemoved().RealNext;

                if (cur.Next.compareAndSet(next, realNext))
                {
                    next = realNext;
                    continue;
                }

                break;
            }

            if (next.X >= x)
                return new WindowSearchResult(new Window(cur, next));

            curRef = cur.Next;

            cur = next;
            next = nextNext.asNode();
        }

        return new WindowSearchResult(curRef);
    }
    /**
     * Trying to skip the part of the list traversed last time.
     */
    private Window getNewStart(AtomicRef<NodeBase> curRef, int x)
    {
        Node cur, next;
        NodeBase curNow = curRef.getValue();

        if (curNow instanceof Node)
        {
            cur = curNow.asNode();
            NodeBase nextNow = cur.Next.getValue();

            if (nextNow instanceof Node)
            {
                next = nextNow.asNode();

                if (next.X < x)
                    return new Window(cur, next);
            }
        }

        cur = _head.getValue().asNode();
        next = cur.Next.getValue().asNode();

        return new Window(cur, next);
    }

    @Override
    public boolean add(int x)
    {
        Window w;
        Node newNode;

        do
        {
            w = findWindow(x);

            if (w.Next.X == x)
                return false;

            newNode = new Node(x, w.Next);
        }
        while(!w.Cur.Next.compareAndSet(w.Next, newNode));

        return true;
    }

    @Override
    public boolean remove(int x)
    {
        while (true)
        {
            Window w = findWindow(x);

            if (w.Next.X != x)
                return false;

            Node cur = w.Cur, next = w.Next;
            NodeBase nextNext = next.Next.getValue();

            if (nextNext instanceof RemovedNode)
                continue;

            RemovedNode newNode = new RemovedNode(nextNext.asNode());

            if (next.Next.compareAndSet(nextNext, newNode))
            {
                cur.Next.compareAndSet(next, nextNext);

                return true;
            }
        }
    }

    @Override
    public boolean contains(int x)
    {
        Window w = findWindow(x);

        return w.Next.X == x;
    }
}