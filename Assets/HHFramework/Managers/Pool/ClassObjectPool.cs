using System;
using System.Collections.Generic;

namespace HHFramework.Managers.Pool
{
    /// <summary>
    /// 类对象池
    /// 1.不能使用带参数的构造函数类，有需求请另写初始化方法
    /// 2.取出或回池时后要将类重置
    /// </summary>
    public class ClassObjectPool : IDisposable
    {
        /// <summary>
        /// 类对象在池中的常驻数量
        /// </summary>
        public Dictionary<int, byte> ClassObjectCount { get; private set; }

        /// <summary>
        /// 类对象池字典
        /// </summary>
        private readonly Dictionary<int, Queue<object>> mClassObjectPoolDic;

#if UNITY_EDITOR
        /// <summary>
        /// 在监视面板中显示的信息
        /// </summary>
        public readonly Dictionary<Type, int> InspectorDic;
#endif

        public ClassObjectPool()
        {
#if UNITY_EDITOR
            InspectorDic = new Dictionary<Type, int>();
#endif
            ClassObjectCount = new Dictionary<int, byte>();
            mClassObjectPoolDic = new Dictionary<int, Queue<object>>();
        }

        #region SetResideCount 设置常驻数量

        /// <summary>
        /// 设置常驻数量
        /// </summary>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        public void SetResideCount<T>(byte count) where T : class
        {
            var key = typeof(T).GetHashCode();
            ClassObjectCount[key] = count;
        }

        #endregion

        #region DequeueClassObject 取出对象

        /// <summary>
        /// 取出对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T DequeueClassObject<T>() where T : class, new()
        {
            lock (mClassObjectPoolDic)
            {
                var key = typeof(T).GetHashCode();
                mClassObjectPoolDic.TryGetValue(key, out var queue);

                if (queue == null)
                {
                    queue = new Queue<object>();
                    mClassObjectPoolDic[key] = queue;
                }

                // 开始获取对象
                if (queue.Count <= 0) return new T();
                var obj = queue.Dequeue();
#if UNITY_EDITOR
                var t = obj.GetType();
                if (InspectorDic.ContainsKey(t))
                {
                    InspectorDic[t]--;
                }
                else
                {
                    InspectorDic[t] = 0;
                }
#endif
                return (T)obj;
            }
        }

        #endregion

        #region EnqueueClassObject 对象回池

        /// <summary>
        /// 对象回池
        /// </summary>
        /// <param name="obj"></param>
        public void EnqueueClassObject(object obj)
        {
            lock (mClassObjectPoolDic)
            {
                var key = obj.GetType().GetHashCode();

                mClassObjectPoolDic.TryGetValue(key, out var queue);
                if (queue == null) return;
                queue.Enqueue(obj);
#if UNITY_EDITOR
                var t = obj.GetType();
                if (InspectorDic.ContainsKey(t))
                {
                    InspectorDic[t]++;
                }
                else
                {
                    InspectorDic[t] = 1;
                }
#endif
            }
        }

        #endregion

        #region Clear 释放类对象池

        /// <summary>
        /// 释放类对象池
        /// </summary>
        public void Clear()
        {
            lock (mClassObjectPoolDic)
            {
                // 定义迭代器
                var enumerator = mClassObjectPoolDic.GetEnumerator();
                // 队列的数量
                while (enumerator.MoveNext())
                {
                    var key = enumerator.Current.Key;
                    // 获取队列
                    var queue = mClassObjectPoolDic[key];
#if UNITY_EDITOR
                    Type t = null;
#endif
                    var queueCount = queue.Count;
                    // 大于常驻数量才释放
                    ClassObjectCount.TryGetValue(key, out var resideCount);
                    while (queueCount > resideCount)
                    {
                        // 队列中有可释放的对象
                        queueCount--;
                        // 从队列中取出后没有引用，变成野指针，等待GC回收
#if UNITY_EDITOR
                        var obj = queue.Dequeue();
                        t = obj.GetType();
                        InspectorDic[t]--;
#endif
                    }

                    if (queueCount != 0) continue;
#if UNITY_EDITOR
                    if (t != null) InspectorDic.Remove(t);
#endif
                }

                // 整个项目,有一处GC即可
                GC.Collect();
            }
        }

        #endregion

        public void Dispose()
        {
            mClassObjectPoolDic.Clear();
        }
    }
}