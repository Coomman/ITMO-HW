import kotlinx.atomicfu.AtomicInt
import kotlinx.atomicfu.atomic
import kotlinx.atomicfu.atomicArrayOfNulls

class Segment
{
    private val _enqIdx: AtomicInt
    private val _deqIdx: AtomicInt = atomic(0)

    val EnqIdx: Int get() = _enqIdx.getAndIncrement() // index for the next enqueue operation
    val DeqIdx: Int get() = _deqIdx.getAndIncrement() // index for the next dequeue operation

    val Data = atomicArrayOfNulls<Any?>(SEGMENT_SIZE)
    val Next = atomic<Segment?>(null)

    // for the first segment creation
    constructor(){
        _enqIdx = atomic(0)
    }

    // each next new segment should be constructed with an element
    constructor(x: Any?)
    {
        _enqIdx = atomic(1)
        Data[0].getAndSet(x)
    }

    val isEmpty: Boolean get() {
        return _deqIdx.value >= _enqIdx.value || _deqIdx.value >= SEGMENT_SIZE
    }
}