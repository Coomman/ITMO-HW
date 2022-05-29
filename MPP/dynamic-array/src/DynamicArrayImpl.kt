@file:Suppress("PropertyName")

import kotlinx.atomicfu.*

private abstract class NodeBase<E>(val Value: E)
private class TransferNode<E>(value: E) : NodeBase<E>(value)
private class Node<E>(value: E) : NodeBase<E>(value)

private class Core<E>(val Capacity: Int)
{
    private val _data = atomicArrayOfNulls<NodeBase<E>>(Capacity)
    val Next = atomic<Core<E>?>(null)

    operator fun get(index: Int) = _data[index].value

    fun changeElement(index: Int, expected: NodeBase<E>?, update: NodeBase<E>): Boolean
    {
        return _data[index].compareAndSet(expected, update)
    }
}

private const val INITIAL_CAPACITY = 1 // DO NOT CHANGE ME

class DynamicArrayImpl<E> : DynamicArray<E>
{
    private val _size = atomic(0)
    private val _array = atomic(Core<E>(INITIAL_CAPACITY))

    override fun get(index: Int): E
    {
        checkIndex(index)

        while (true)
        {
            val array = _array.value
            val node = array[index]

            if (node is TransferNode)
            {
                transferElements(array)
                continue
            }

            return node!!.Value
        }
    }

    override fun put(index: Int, element: E)
    {
        checkIndex(index)

        while (true)
        {
            val array = _array.value
            val node = array[index]

            if (node is TransferNode)
            {
                transferElements(array)
                continue
            }

            if (array.changeElement(index, node, Node(element))) return
        }
    }

    override fun pushBack(element: E)
    {
        while (true)
        {
            val array = _array.value
            val curSize = size

            if (curSize >= array.Capacity)
            {
                transferElements(array)
                continue
            }

            if (array.changeElement(curSize, null, Node(element)))
            {
                _size.incrementAndGet()
                return
            }
        }
    }

    override val size: Int get() = _size.value


    private fun transferElements(array: Core<E>)
    {
        array.Next.compareAndSet(null, Core(array.Capacity * 2))

        val nextCore = array.Next.value!!
        for (i in 0 until array.Capacity)
        {
            var node = array[i]!!

            while (node is Node && !array.changeElement(i, node, TransferNode(node.Value)))
            {
                node = array[i]!!
            }

            nextCore.changeElement(i, null, Node(node.Value))
        }

        _array.compareAndSet(array, nextCore)
    }

    private fun checkIndex(index: Int)
    {
        if (index >= size)
            throw IllegalArgumentException("Index out of range: $index")
    }
}