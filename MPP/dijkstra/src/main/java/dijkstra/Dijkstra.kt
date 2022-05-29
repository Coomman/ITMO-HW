package dijkstra

import java.util.concurrent.Phaser
import java.util.concurrent.atomic.AtomicInteger
import kotlin.Comparator
import kotlin.concurrent.thread

private val NODE_DISTANCE_COMPARATOR = Comparator<Node> { o1, o2 -> o1!!.distance.compareTo(o2!!.distance) }

// Returns `Integer.MAX_VALUE` if a path has not been found.
fun shortestPathParallel(start: Node) {
    val workers = Runtime.getRuntime().availableProcessors()
    // The distance to the start node is `0`
    start.distance = 0

    // Create a priority (by distance) queue and add the start node into it
    val q = MultiQueue(workers, NODE_DISTANCE_COMPARATOR)
    q.push(start)

    val activeNodes = AtomicInteger(1)
    // Run worker threads and wait until the total work is done
    val onFinish = Phaser(workers + 1) // `arrive()` should be invoked at the end by each worker

    repeat(workers) {
        thread {
            while (activeNodes.get() > 0)
            {
                val curVertex = q.pop() ?: continue

                for (edge in curVertex.outgoingEdges)
                {
                    while(true)
                    {
                        val nextNode = edge.to

                        val curDist = nextNode.distance
                        val newDist = curVertex.distance + edge.weight

                        if (curDist <= newDist)
                            break

                        if (nextNode.casDistance(curDist, newDist))
                        {
                            activeNodes.incrementAndGet()
                            q.push(nextNode)

                            break
                        }
                    }
                }

                activeNodes.decrementAndGet()
            }

            onFinish.arrive()
        }
    }

    onFinish.arriveAndAwaitAdvance()
}