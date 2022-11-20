namespace HHFramework
{
    /// <summary>
    /// 时间组件
    /// </summary>
    public class TimeComponent : HHBaseComponent, IUpdateComponent
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);

            mTimeManager = new TimeManager();
        }

        #region 定时器

        /// <summary>
        /// 定时器管理器
        /// </summary>
        private TimeManager mTimeManager;

        /// <summary>
        /// 注册定时器
        /// </summary>
        /// <param name="timeAction"></param>
        internal void RegisterTimeAction(TimeAction timeAction)
        {
            mTimeManager.RegisterTimeAction(timeAction);
        }

        /// <summary>
        /// 移除定时器
        /// </summary>
        /// <param name="timeAction"></param>
        internal void RemoveTimeAction(TimeAction timeAction)
        {
            GameEntry.Pool.EnqueueClassObject(timeAction);
        }

        /// <summary>
        /// 创建定时器
        /// </summary>
        /// <returns></returns>
        public TimeAction CreateTimeAction()
        {
            return GameEntry.Pool.DequeueClassObject<TimeAction>();
        }

        #endregion

        public override void ShutDown()
        {
            mTimeManager.Dispose();
        }

        public void OnUpdate()
        {
            mTimeManager.OnUpdate();
        }
    }
}