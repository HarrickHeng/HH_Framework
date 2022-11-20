namespace HHFramework
{
    /// <summary>
    /// float变量
    /// </summary>
    public class VarFloat : Variable<float>
    {
        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <returns></returns>
        public static VarFloat Alloc()
        {
            var var = GameEntry.Pool.DequeueVarObject<VarFloat>();
            var.Value = 0;
            var.Retain();
            return var;
        }

        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <param name="value">初始值</param>
        /// <returns></returns>
        public static VarFloat Alloc(float value)
        {
            var var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarFloat -> float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator float(VarFloat value)
        {
            return value.Value;
        }
    }
}