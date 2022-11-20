using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// GameObject变量
    /// </summary>
    public class VarGameObject : Variable<GameObject>
    {
        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <returns></returns>
        public static VarGameObject Alloc()
        {
            var var = GameEntry.Pool.DequeueVarObject<VarGameObject>();
            var.Value = null;
            var.Retain();
            return var;
        }

        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <param name="value">初始值</param>
        /// <returns></returns>
        public static VarGameObject Alloc(GameObject value)
        {
            var var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarGameObject -> GameObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator GameObject(VarGameObject value)
        {
            return value.Value;
        }
    }
}