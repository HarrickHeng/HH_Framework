namespace HHFramework
{
    /// <summary>
    /// 事件组件
    /// </summary>
    public class EventComponent : HHBaseComponent
    {
        /// <summary>
        /// 事件管理器
        /// </summary>
        private EventManager mEventManager;

        /// <summary>
        /// Socket事件
        /// </summary>
        public SocketEvent SocketEvent;

        /// <summary>
        /// 通用事件
        /// </summary>
        public CommonEvent CommonEvent;

        protected override void OnAwake()
        {
            base.OnAwake();
            mEventManager = new EventManager();
            SocketEvent = mEventManager.SocketEvent;
            CommonEvent = mEventManager.CommonEvent;
        }

        public override void ShutDown()
        {
            mEventManager.Dispose();
        }
    }
}