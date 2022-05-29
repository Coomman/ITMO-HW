/**
 * @author : Rauba Maksim
 */

class Solution : AtomicCounter
{
    private class Node(value: Int)
    {
        val Value = value
        val Next = Consensus<Node>()
    }

    private val _root = Node(0)
    private val _localVersion = ThreadLocal.withInitial { _root }

    override fun getAndAdd(x: Int): Int
    {
        var oldValue: Int

        do
        {
            val local = _localVersion.get()

            oldValue = local.Value
            val node = Node(oldValue + x)

            _localVersion.set(local.Next.decide(node))
        }
        while(_localVersion.get() != node)

        return oldValue
    }
}
