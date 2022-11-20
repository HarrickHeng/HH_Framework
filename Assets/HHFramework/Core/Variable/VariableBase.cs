using System;

namespace HHFramework
{
    /// <summary>
    /// 变量基类
    /// </summary>
    public abstract class VariableBase
    {
        /// <summary>
        /// 获取变量类型
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// 引用计数
        /// </summary>
        public byte ReferenceCount { get; private set; }

        /// <summary>
        /// 保留对象
        /// </summary>
        public void Retain()
        {
            ReferenceCount++;
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public void Release()
        {
            ReferenceCount--;
            if (ReferenceCount < 1)
            {
                // 对象回池
                GameEntry.Pool.EnqueueVarObject(this);
            }
        }
    }
}