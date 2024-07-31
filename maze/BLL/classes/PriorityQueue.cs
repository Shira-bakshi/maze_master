using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.classes
{
    public class PriorityQueue<vertax>
    {
        private Vertex[] heap;
        private int size;
        private int capacity;

        public PriorityQueue(int capacity)
        {
            this.capacity = capacity;
            size = 0;
            heap = new Vertex[capacity];
        }
        public bool IsEmpty { get { return size == 0; } }
        private int Parent(int i) => (i - 1) / 2;
        private int LeftChild(int i) => 2 * i + 1;
        private int RightChild(int i) => 2 * i + 2;

        private void Swap(int i, int j)
        {
            Vertex temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        private void HeapifyUp(int index)
        {
            int current = index;
            while (current > 0 && heap[current].costTotal <= heap[Parent(current)].costTotal)
            {
                if (heap[current].costTotal == heap[Parent(current)].costTotal)
                {
                    if (heap[current].costEnd < heap[Parent(current)].costEnd)
                    {
                        Swap(current, Parent(current));
                        current = Parent(current);
                    }
                }
                else
                {
                    Swap(current, Parent(current));
                    current = Parent(current);
                }
                //Console.WriteLine("the heapify up:");
                //Console.WriteLine(heap[current].index + " "+heap[current].cost + " < " + heap[Parent(current)].cost+" " + heap[Parent(current)].index);
                Swap(current, Parent(current));
                current = Parent(current);
            }
        }

        private void HeapifyDown(int index)
        {
            int minIndex = index;
            int left = LeftChild(index);
            int right = RightChild(index);

            if (left < size && heap[left].costTotal <= heap[minIndex].costTotal)
            {
                if (heap[left].costTotal == heap[minIndex].costTotal)
                {
                    if (heap[left].costEnd < heap[minIndex].costEnd)
                        minIndex = left;
                }
                else
                    minIndex = left;
                //Console.WriteLine("the heapify down:");
                //Console.WriteLine(heap[left].index + " " + heap[left].cost + " < " + heap[minIndex].cost + " " + heap[minIndex]);
            }
            if (right < size && heap[right].costTotal <= heap[minIndex].costTotal)
            {
                if (heap[right].costTotal == heap[minIndex].costTotal)
                {
                    if (heap[right].costEnd < heap[minIndex].costEnd)
                        minIndex = right;
                }
                else
                    minIndex = right;
                //Console.WriteLine("the heapify down:");
                //Console.WriteLine(heap[right].index+ " "+heap[right].cost + " < " + heap[minIndex].cost+" "+ heap[minIndex]);

            }
            if (index != minIndex)
            {
                Swap(index, minIndex);
                HeapifyDown(minIndex);
            }
        }

        public void Insert(Vertex value)
        {
            if (size == capacity)
            {
                Console.WriteLine("Heap is full. Cannot insert more elements.");
                return;
            }

            size++;
            heap[size - 1] = value;
            HeapifyUp(size - 1);

        }

        public Vertex ExtractMin()
        {
            if (size == 0)
            {
                Console.WriteLine("Heap is empty.");
                return null; // Assuming the minimum value for integer.
            }
            Vertex min = heap[0];
            heap[0] = heap[size - 1];
            size--;
            HeapifyDown(0);
            return min;
        }
        public bool Contains(Vertex e)
        {
            for (int i = 0; i < size; i++)
                if (e == heap[i])
                    return true;
            return false;
        }
        public void Remove(Vertex v)
        {
            if (IsEmpty)
            {
                Console.WriteLine("Priority queue is empty");
            }
            for (int i = 0; i < size; i++)
                if (this.heap[i].index == v.index)
                {
                    this.heap[i] = this.heap[size - 1];
                    this.heap[size - 1] = null;
                    size--;
                    HeapifyDown(i);
                    break;
                }
        }
        public void update(Vertex e)
        {
            for (int i = 0; i < size; i++)
            {
                if (heap[i].index == e.index)
                {
                    heap[i] = e;
                    if (i != 0 && heap[i].costTotal < heap[Parent(i)].costTotal)
                    {
                        HeapifyUp(i);
                    }
                    else
                        HeapifyDown(i);
                    break;
                }
            }
        }
        public void Print()
        {
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(heap[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
