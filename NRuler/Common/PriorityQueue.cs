using System.Collections.Generic;

namespace NRuler.Common
{
    /// <summary>
    /// Summary of running times
    ///             Linked  List        Binary Tree (Min-)Heap Fibonacci Heap 
    /// insert      O(1)                O(log n)    O(log n)    O(1) 
    /// accessmin   O(n)                O(1)        O(1)        O(1) 
    /// deletemin   O(n)                O(log n)    O(log n)    O(log n)* 
    /// decreasekey O(1)                O(log n)    O(log n)    O(1)* 
    /// delete      O(n)                O(n)        O(log n)    O(log n)* 
    /// merge       O(1)                O(m log(n+m)) O(m log(n+m)) O(1) 
    /// (*)Amortized time
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        // foamliu, 2008/11/23, use List for now.
        // To get better performance, typically use a heap (such as a Fibonacci heap) as backbone. 


        //private IComparer<T> m_strategy;
        //private List<T> m_list;
        private BinaryHeap<T> m_list;


        /// <summary>
        /// add an element to the queue with an associated priority 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pri"></param>
        public void Insert(T value)
        {
            // a simple, inefficient ways to implement.
            //m_list.Add(obj);


            // foamliu, 2008/11/23, sort the conflict set 
            //m_list.Sort(m_strategy);

            m_list.Insert(value);
        }

        /// <summary>
        /// remove the element from the queue that has the highest priority, and return it 
        /// </summary>
        /// <returns>null means no more.</returns>
        public T Remove()
        {
            //if (m_list.Count > 0)
            //{
            //    T obj = m_list[0];
            //    m_list.Remove(obj);
            //    return obj;
            //}
            //else
            //    return default(T);     

            return (T)m_list.Remove();

        }

        //public void Remove(T obj)
        //{
        //    // TODO: add this.
        //    //if (m_list.Contains(obj))
        //    //{
        //    //    m_list.Remove(obj);
        //    //}
        //}

        public PriorityQueue(IComparer<T> strategy)
        {
            //this.m_strategy = strategy;
            //this.m_list = new List<T>();
            m_list = new BinaryHeap<T>(strategy);

        }


    } 
}
