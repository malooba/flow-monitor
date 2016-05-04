//Copyright 2016 Malooba Ltd

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System.Runtime.CompilerServices;

namespace Diagram.DiagramModel
{
    public sealed class PriorityQueue
    {
        private RoutePoint[] nodes;
        private long nodeCounter;

        /// <summary>
        /// Instantiate a new Priority Queue
        /// </summary>
        /// <param name="capacity">Initial capacity</param>
        public PriorityQueue(int capacity)
        {
            Count = 0;
            Capacity = capacity;
            nodeCounter = 0;
        }

        /// <summary>
        /// Returns the number of nodes in the queue.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Queue capacity
        /// </summary>
        public int Capacity
        {
            get { return nodes.Length - 1; }
            private set
            {
                var newNodes = new RoutePoint[value];
                nodes?.CopyTo(newNodes, 0);
                nodes = newNodes;
            }
        }

        /// <summary>
        /// Removes every node from the queue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public void Clear()
        {
            Count = 0;
        }

        /// <summary>
        /// Returns whether the given node is in the queue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(RoutePoint node)
        {
            return (nodes[node.QueueIndex] == node);
        }

        /// <summary>
        /// Enqueue a node
        /// </summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(RoutePoint node)
        {
            if(++Count > Capacity) Capacity = Capacity * 2;
            nodes[Count] = node;
            node.QueueIndex = Count;
            node.InsertionIndex = nodeCounter++;
            CascadeUp(nodes[Count]);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Swap(RoutePoint node1, RoutePoint node2)
        {
            //Swap the nodes
            nodes[node1.QueueIndex] = node2;
            nodes[node2.QueueIndex] = node1;

            //Swap their indicies
            var temp = node1.QueueIndex;
            node1.QueueIndex = node2.QueueIndex;
            node2.QueueIndex = temp;
        }

        private void CascadeUp(RoutePoint node)
        {
            //aka Heapify-up
            var parent = node.QueueIndex / 2;
            while(parent >= 1)
            {
                var parentNode = nodes[parent];
                if(HasPriority(parentNode, node))
                    break;

                //Node has y priority value, so move it up the heap
                Swap(node, parentNode); //For some reason, this is faster with Swap() rather than (less..?) individual operations, like in CascadeDown()

                parent = node.QueueIndex / 2;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CascadeDown(RoutePoint node)
        {
            var finalQueueIndex = node.QueueIndex;
            while(true)
            {
                var newParent = node;
                var childLeftIndex = 2 * finalQueueIndex;

                //Check if the left-child is x-priority than the current node
                if(childLeftIndex > Count)
                {
                    //This could be placed outside the loop, but then we'd have to check newParent != node twice
                    node.QueueIndex = finalQueueIndex;
                    nodes[finalQueueIndex] = node;
                    break;
                }

                var childLeft = nodes[childLeftIndex];
                if(HasPriority(childLeft, newParent))
                {
                    newParent = childLeft;
                }

                //Check if the right-child is x-priority than either the current node or the left child
                var childRightIndex = childLeftIndex + 1;
                if(childRightIndex <= Count)
                {
                    var childRight = nodes[childRightIndex];
                    if(HasPriority(childRight, newParent))
                    {
                        newParent = childRight;
                    }
                }

                //If either of the children has x (smaller) priority, swap and continue cascading
                if(newParent != node)
                {
                    nodes[finalQueueIndex] = newParent;

                    var temp = newParent.QueueIndex;
                    newParent.QueueIndex = finalQueueIndex;
                    finalQueueIndex = temp;
                }
                else
                {
                    node.QueueIndex = finalQueueIndex;
                    nodes[finalQueueIndex] = node;
                    break;
                }
            }
        }

        /// <summary>
        /// Returns true if x has x priority over y, false otherwise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasPriority(RoutePoint x, RoutePoint y)
        {
            return (x.Priority < y.Priority || (x.Priority == y.Priority && x.InsertionIndex < y.InsertionIndex));
        }

        /// <summary>
        /// Removes the head of the queue (node with highest priority; ties are broken by order of insertion), and returns it.  O(log n)
        /// </summary>
        public RoutePoint Dequeue()
        {
            var node = nodes[1];
            Remove(node);
            return node;
        }

        /// <summary>
        /// Returns the head of the queue, without removing it
        /// </summary>
        public RoutePoint Peek => nodes[1];

        /// <summary>
        /// This method must be called on a node every time its priority changes while it is in the queue.  
        /// <b>Forgetting to call this method will result in a corrupted queue!</b>
        /// O(log n)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdatePriority(RoutePoint node)
        {
            //Bubble the updated node up or down as appropriate
            var parentIndex = node.QueueIndex / 2;
            var parentNode = nodes[parentIndex];

            if(parentIndex > 0 && HasPriority(node, parentNode))
            {
                CascadeUp(node);
            }
            else
            {
                //Note that CascadeDown will be called if parentNode == node (that is, node is the root)
                CascadeDown(node);
            }
        }

        /// <summary>
        /// Removes a node from the queue.  Note that the node does not need to be the head of the queue.  O(log n)
        /// </summary>
        public void Remove(RoutePoint node)
        {
            if(!Contains(node))
            {
                return;
            }

            if(Count <= 1)
            {
                nodes[1] = null;
                Count = 0;
                return;
            }

            //Make sure the node is the last node in the queue
            var wasSwapped = false;
            var formerLastNode = nodes[Count];
            if(node.QueueIndex != Count)
            {
                //Swap the node with the last node
                Swap(node, formerLastNode);
                wasSwapped = true;
            }

            Count--;
            nodes[node.QueueIndex] = null;

            if(wasSwapped)
            {
                //Now bubble formerLastNode (which is no longer the last node) up or down as appropriate
                UpdatePriority(formerLastNode);
            }
        }

        /// <summary>
        /// <b>Should not be called in production code.</b>
        /// Checks to make sure the queue is still in a valid state.  Used for testing/debugging the queue.
        /// </summary>
        public bool IsValidQueue()
        {
            for(var i = 1; i < nodes.Length; i++)
            {
                if(nodes[i] == null) continue;
                var childLeftIndex = 2 * i;
                if(childLeftIndex < nodes.Length && nodes[childLeftIndex] != null && HasPriority(nodes[childLeftIndex], nodes[i]))
                    return false;

                var childRightIndex = childLeftIndex + 1;
                if(childRightIndex < nodes.Length && nodes[childRightIndex] != null && HasPriority(nodes[childRightIndex], nodes[i]))
                    return false;
            }
            return true;
        }
    }
}