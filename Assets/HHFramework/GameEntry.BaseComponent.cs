using System;
using System.Collections.Generic;

namespace HHFramework
{
    /// <summary>
    /// 基础组件管理
    /// </summary>
    public partial class GameEntry
    {
        /// <summary>
        /// 基础组件列表
        /// </summary>
        private static readonly LinkedList<HHBaseComponent> MBaseComponents = new LinkedList<HHBaseComponent>();

        #region 组件属性

        /// <summary>
        /// 数据组件
        /// </summary>
        public static DataComponent Data { get; private set; }

        /// <summary>
        /// 配置表组件
        /// </summary>
        public static DataTableComponent DataTable { get; private set; }

        /// <summary>
        /// 下载组件
        /// </summary>
        public static DownloadComponent Download { get; private set; }

        /// <summary>
        /// 事件组件
        /// </summary>
        public static EventComponent Event { get; private set; }

        /// <summary>
        /// 状态机组件
        /// </summary>
        public static FsmComponent Fsm { get; private set; }

        /// <summary>
        /// 游戏对象组件
        /// </summary>
        public static GameObjComponent GameObj { get; private set; }

        /// <summary>
        /// Http组件
        /// </summary>
        public static HttpComponent Http { get; private set; }

        /// <summary>
        /// 本地化组件
        /// </summary>
        public static LocalizationComponent Localization { get; private set; }

        /// <summary>
        /// 对象池组件
        /// </summary>
        public static PoolComponent Pool { get; private set; }

        /// <summary>
        /// 流程组件
        /// </summary>
        public static ProcedureComponent Procedure { get; private set; }

        /// <summary>
        /// 资源组件
        /// </summary>
        public static ResourceComponent Resource { get; private set; }

        /// <summary>
        /// 场景组件
        /// </summary>
        public static SceneComponent Scene { get; private set; }

        /// <summary>
        /// 设置组件
        /// </summary>
        public static SettingComponent Setting { get; private set; }

        /// <summary>
        /// 时间组件
        /// </summary>
        public static TimeComponent Time { get; private set; }

        /// <summary>
        /// UI组件
        /// </summary>
        public static UIComponent UI { get; private set; }
        
        /// <summary>
        /// Socket组件
        /// </summary>
        public static SocketComponent Socket { get; private set; }

        #endregion

        #region RegisterBaseComponent 注册基础组件

        /// <summary>
        /// 注册基础组件
        /// </summary>
        /// <param name="component"></param>
        internal static void RegisterBaseComponent(HHBaseComponent component)
        {
            // 获取组件类型
            var type = component.GetType();
            var curr = MBaseComponents.First;

            while (curr != null)
            {
                if (curr.Value.GetType() == type) return;
                curr = curr.Next;
            }

            // 组件加入最后一个节点
            MBaseComponents.AddLast(component);
        }

        #endregion

        #region GetBaseComponent 获取基础组件

        internal static T GetBaseComponent<T>() where T : HHBaseComponent
        {
            return (T)GetBaseComponent(typeof(T));
        }

        /// <summary>
        ///  获取基础组件
        /// </summary>
        /// <param name="type"></param>
        internal static HHBaseComponent GetBaseComponent(Type type)
        {
            var curr = MBaseComponents.First;

            while (curr != null)
            {
                if (curr.Value.GetType() == type)
                {
                    return curr.Value;
                }

                curr = curr.Next;
            }

            return null;
        }

        #endregion

        #region InitBaseComponents 初始化基础组件

        /// <summary>
        /// 初始化基础组件
        /// </summary>
        private static void InitBaseComponents()
        {
            Data = GetBaseComponent<DataComponent>();
            DataTable = GetBaseComponent<DataTableComponent>();
            Download = GetBaseComponent<DownloadComponent>();
            Event = GetBaseComponent<EventComponent>();
            Fsm = GetBaseComponent<FsmComponent>();
            GameObj = GetBaseComponent<GameObjComponent>();
            Http = GetBaseComponent<HttpComponent>();
            Localization = GetBaseComponent<LocalizationComponent>();
            Pool = GetBaseComponent<PoolComponent>();
            Procedure = GetBaseComponent<ProcedureComponent>();
            Resource = GetBaseComponent<ResourceComponent>();
            Scene = GetBaseComponent<SceneComponent>();
            Setting = GetBaseComponent<SettingComponent>();
            Time = GetBaseComponent<TimeComponent>();
            UI = GetBaseComponent<UIComponent>();
            Socket = GetBaseComponent<SocketComponent>();
        }

        #endregion
    }
}