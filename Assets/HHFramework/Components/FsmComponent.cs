namespace HHFramework
{
    /// <summary>
    /// 状态机组件
    /// </summary>
    public class FsmComponent : HHBaseComponent
    {
        /// <summary>
        /// 状态机管理器
        /// </summary>
        private FsmManager mFsmManager;

        /// <summary>
        /// 状态机临时编号
        /// </summary>
        private int mTempFsmId = 0;

        protected override void OnAwake()
        {
            base.OnAwake();
            mFsmManager = new FsmManager();
        }

        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <param name="owner">拥有者</param>
        /// <param name="states">状态数组</param>
        /// <typeparam name="T">拥有者类型</typeparam>
        /// <returns></returns>
        public Fsm<T> Create<T>(T owner, FsmState<T>[] states) where T : class
        {
            return mFsmManager.Create(mTempFsmId++, owner, states);
        }

        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <param name="fsmId"></param>
        public void DestroyFsm(int fsmId)
        {
            mFsmManager.DestroyFsm(fsmId);
        }

        public override void ShutDown()
        {
            mFsmManager.Dispose();
        }
    }
}