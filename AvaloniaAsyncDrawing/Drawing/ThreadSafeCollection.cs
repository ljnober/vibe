// 线程安全集合接口定义
using System;
using System.Collections;
using System.Collections.Generic;

namespace AvaloniaAsyncDrawing.Drawing
{
    /// <summary>
    /// 线程安全集合接口，支持多线程安全读写，常用于异步渲染数据快照与主线程数据同步。
    /// </summary>
    /// <typeparam name="T">集合元素类型</typeparam>
    public interface IThreadSafeCollection<T> : IEnumerable<T>
    {
        /// <summary>
        /// 获取集合元素数量。
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 添加元素，线程安全。
        /// </summary>
        /// <param name="item">要添加的元素</param>
        void Add(T item);

        /// <summary>
        /// 移除元素，线程安全。
        /// </summary>
        /// <param name="item">要移除的元素</param>
        /// <returns>是否移除成功</returns>
        bool Remove(T item);

        /// <summary>
        /// 清空集合，线程安全。
        /// </summary>
        void Clear();

        /// <summary>
        /// 获取集合快照（深拷贝），用于异步渲染线程安全读取。
        /// </summary>
        /// <returns>元素快照数组</returns>
        T[] Snapshot();
    }

    /// <summary>
    /// 线程安全泛型集合实现，支持多线程安全操作与快照。
    /// </summary>
    /// <typeparam name="T">集合元素类型</typeparam>
    public class ThreadSafeCollection<T> : IThreadSafeCollection<T>, IEnumerable
    {
        private readonly List<T> _list = new List<T>();
        private readonly object _syncRoot = new object();

        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    return _list.Count;
                }
            }
        }

        public void Add(T item)
        {
            lock (_syncRoot)
            {
                _list.Add(item);
            }
        }

        public bool Remove(T item)
        {
            lock (_syncRoot)
            {
                return _list.Remove(item);
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _list.Clear();
            }
        }

        public T[] Snapshot()
        {
            lock (_syncRoot)
            {
                return _list.ToArray();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            // 快照枚举，避免并发异常
            return Snapshot().AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}