package stack;

import kotlinx.atomicfu.AtomicRef;

import java.util.ArrayList;
import java.util.Random;

public class StackImpl implements Stack
{
    private static class Node
    {
        final int Value;
        final AtomicRef<Node> Next;

        Node(int value, Node next)
        {
            this.Value = value;
            this.Next = new AtomicRef<>(next);
        }
    }

    private final int ELIMINATION_ARRAY_SIZE = 10;
    private final int PUSH_ELIMINATION_INSERT_TRIES = 3;
    private final int PUSH_ELIMINATION_CHECKS = 3;
    private final int POP_ELIMINATION_TRIES = 3;

    private final ArrayList<AtomicRef<Integer>> _eliminationArray = new ArrayList<>(ELIMINATION_ARRAY_SIZE);
    private final Random _rand = new Random();

    private final AtomicRef<Node> _head = new AtomicRef<>(null);

    public StackImpl()
    {
        for (int i = 0; i < ELIMINATION_ARRAY_SIZE; ++i)
        {
            _eliminationArray.add(new AtomicRef<Integer>(null));
        }
    }

    @Override
    public void push(int x)
    {
        Integer boxedInt = x;

        for (int i = 0; i < PUSH_ELIMINATION_INSERT_TRIES; ++i)
        {
            int index = _rand.nextInt(ELIMINATION_ARRAY_SIZE);

            if (tryElimination(index, boxedInt))
                return;

            if (index - 1 > -1 && tryElimination(index - 1, boxedInt))
                return;

            if (index + 1 < ELIMINATION_ARRAY_SIZE && tryElimination(index + 1, boxedInt))
                return;
        }

        pushCore(x);
    }
    private boolean tryElimination(int index, Integer boxedInt)
    {
        AtomicRef<Integer> curEl = _eliminationArray.get(index);

        if (curEl.compareAndSet(null, boxedInt))
        {
            for (int i = 0; i < PUSH_ELIMINATION_CHECKS; ++i)
                if (curEl.getValue() == null)
                    return true;

            return !curEl.compareAndSet(boxedInt, null);
        }

        return false;
    }
    private void pushCore(int x)
    {
        Node curHead, newHead;

        do
        {
            curHead = _head.getValue();
            newHead = new Node(x, curHead);
        }
        while(!_head.compareAndSet(curHead, newHead));
    }

    @Override
    public int pop()
    {
        for (int i = 0; i < POP_ELIMINATION_TRIES; ++i){
            int index = _rand.nextInt(ELIMINATION_ARRAY_SIZE);
            Integer value = tryEliminate(index);

            if (value != null)
                return value;
        }

        return popCore();
    }

    private Integer tryEliminate(int index)
    {
        Integer value = _eliminationArray.get(index).getAndSet(null);
        if (value != null)
            return value;

        if (index - 1 > -1)
        {
            value = _eliminationArray.get(index - 1).getAndSet(null);
            if (value != null)
                return value;
        }

        if (index + 1 < ELIMINATION_ARRAY_SIZE)
        {
            value = _eliminationArray.get(index + 1).getAndSet(null);
            return value;
        }

        return null;
    }
    private int popCore()
    {
        Node curHead;

        do
        {
            curHead = _head.getValue();

            if (curHead == null)
                return Integer.MIN_VALUE;
        }
        while(!_head.compareAndSet(curHead, curHead.Next.getValue()));

        return curHead.Value;
    }
}
