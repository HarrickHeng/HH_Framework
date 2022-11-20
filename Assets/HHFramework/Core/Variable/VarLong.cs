namespace HHFramework
{
    /// <summary>
    /// long变量
    /// </summary>
    public class VarLong : Variable<long>
    {
        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <returns></returns>
        public static VarLong Alloc()
        {
            var var = GameEntry.Pool.DequeueVarObject<VarLong>();
            var.Value = 0;
            var.Retain();
            return var;
        }

        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <param name="value">初始值</param>
        /// <returns></returns>
        public static VarLong Alloc(long value)
        {
            var var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarLong -> long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator long(VarLong value)
        {
            return value.Value;
        }
    }
}