using System.Collections.Generic;

namespace HHFramework
{
    /// <summary>
    /// 更新组件管理
    /// </summary>
    public partial class GameEntry
    {
        /// <summary>
        /// 更新组件列表
        /// </summary>
        private static readonly LinkedList<IUpdateComponent> m_UpdateComponents = new LinkedList<IUpdateComponent>();

        #region RegisterUpdateComponent 注册更新组件

        /// <summary>
        /// 注册更新组件
        /// </summary>
        /// <param name="component"></param>
        public static void RegisterUpdateComponent(IUpdateComponent component)
        {
            m_UpdateComponents.AddLast(component);
        }

        #endregion

        #region RemoveUpdateComponent 移除更新组件

        /// <summary>
        /// 移除更新组件
        /// </summary>
        /// <param name="component"></param>
        public static void RemoveUpdateComponent(IUpdateComponent component)
        {
            m_UpdateComponents.Remove(component);
        }

        #endregion
    }
}