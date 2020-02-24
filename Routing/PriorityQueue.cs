using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    class PriorityQueue
    {
        //class Pair
        //{
        //    readonly private int priority;
        //    readonly private int value;

        //    public Pair(int priority, int value)
        //    {
        //        this.priority = priority;
        //        this.value = value;
        //    }

        //    public int Priority
        //    {
        //        get
        //        {
        //            return priority;
        //        }
        //    }

        //    public int Value
        //    {
        //        get
        //        {
        //            return value;
        //        }
        //    }
        //}

        private readonly List<KeyValuePair<int, int>> Data;
        public int Count { get { return Data.Count; } }

        public PriorityQueue()
        {
            Data = new List<KeyValuePair<int, int>>();
        }

        public void Enqueue(int priority, int value)
        {
            Data.Add(new KeyValuePair<int, int>(priority, value));
            int i = Count - 1;
            int parent = (i - 1) / 2;
            while (Data[i].Key < Data[parent].Key && i > 0)
            {
                Swap(i, parent);
                i = parent;
                parent = (i - 1) / 2;
            }
        }

        public int Deque()
        {
            if (Data.Count > 0)
            {
                int result = Data[0].Value;
                Data[0] = Data[Count - 1];
                Data.RemoveAt(Count - 1);
                Heapify(0);
                return result;
            }
            else throw new IndexOutOfRangeException("Queue is empty");
        }

        private void Swap(int firstIndex, int secondIndex)
        {
            KeyValuePair<int, int> temp = Data[firstIndex];
            Data[firstIndex] = Data[secondIndex];
            Data[secondIndex] = temp;
        }

        private void Heapify(int i)
        {
            int leftChild;
            int rightChild;
            int minChild;

            for (; ; )
            {
                leftChild = 2 * i + 1;
                rightChild = 2 * i + 2;
                minChild = i;
                if (leftChild < Count && Data[leftChild].Key < Data[minChild].Key)
                    minChild = leftChild;
                if (rightChild < Count && Data[rightChild].Key < Data[minChild].Key)
                    minChild = rightChild;
                if (minChild == i)
                    break;

                Swap(minChild, i);

                i = minChild;
            }
        }

        public void Clear()
        {
            Data.Clear();
        }
    }
}
