using System;

namespace HHFramework
{
    /// <summary>
    /// 变量泛型
    /// 1.一个Retain方法保留需要对应一个Release方法释放
    /// 2.异步协程里需要先使用Retain方法保留变量对象，同步则不用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Variable<T> : VariableBase
    {
        /// <summary>
        /// 当前存储的真实值
        /// </summary>
        public T Value;

        /// <summary>
        /// 变量类型
        /// </summary>
        public override Type Type => typeof(T);
    }
}

