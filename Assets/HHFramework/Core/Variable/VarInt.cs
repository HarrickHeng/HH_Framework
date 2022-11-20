namespace HHFramework
{
    /// <summary>
    /// int变量
    /// </summary>
    public class VarInt : Variable<int>
    {
        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <returns></returns>
        public static VarInt Alloc()
        {
            var var = GameEntry.Pool.DequeueVarObject<VarInt>();
            var.Value = 0;
            var.Retain();
            return var;
        }

        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <param name="value">初始值</param>
        /// <returns></returns>
        public static VarInt Alloc(int value)
        {
            var var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt -> int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator int(VarInt value)
        {
            return value.Value;
        }
    }
}