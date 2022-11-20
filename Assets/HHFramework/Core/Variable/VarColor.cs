using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// Color变量
    /// </summary>
    public class VarColor : Variable<Color>
    {
        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <returns></returns>
        public static VarColor Alloc()
        {
            var var = GameEntry.Pool.DequeueVarObject<VarColor>();
            var.Value = Color.clear;
            var.Retain();
            return var;
        }

        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <param name="value">初始值</param>
        /// <returns></returns>
        public static VarColor Alloc(Color value)
        {
            var var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarColor -> Color
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Color(VarColor value)
        {
            return value.Value;
        }
    }
}