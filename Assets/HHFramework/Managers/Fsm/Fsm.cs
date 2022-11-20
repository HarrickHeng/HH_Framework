using System.Collections.Generic;

namespace HHFramework
{
    /// <summary>
    /// 状态机
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Fsm<T> : FsmBase where T : class
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        private FsmState<T> mCurrState;

        /// <summary>
        /// 状态字典
        /// </summary>
        private readonly Dictionary<byte, FsmState<T>> mStateDic;

        /// <summary>
        /// 参数字典
        /// </summary>
        private readonly Dictionary<string, VariableBase> mParamDic;

        /// <summary>
        /// 拥有者
        /// </summary>
        public T Owner { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fsmId">状态机编号</param>
        /// <param name="owner">拥有者</param>
        /// <param name="status">状态数组</param>
        public Fsm(int fsmId, T owner, FsmState<T>[] status) : base(fsmId)
        {
            mStateDic = new Dictionary<byte, FsmState<T>>();
            mParamDic = new Dictionary<string, VariableBase>();
            Owner = owner;

            // 状态加入字典
            for (int i = 0, len = status.Length; i < len; i++)
            {
                var state = status[i];
                state.CurrFsm = this;
                mStateDic[(byte)i] = state;
            }

            // 设置默认状态
            CurrStateType = 0;
            mCurrState = mStateDic[CurrStateType];
            mCurrState.OnEnter();
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        public FsmState<T> GetState(byte stateType)
        {
            mStateDic.TryGetValue(stateType, out var state);
            return state;
        }

        public void OnUpdate()
        {
            mCurrState?.OnUpdate();
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(byte newState)
        {
            // 两个状态一样 不重复进入
            if (CurrStateType == newState) return;

            mCurrState?.OnLeave();

            CurrStateType = newState;
            mCurrState = mStateDic[CurrStateType];

            // 进入新状态
            mCurrState.OnEnter();
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TData">泛型类型</typeparam>
        public void SetData<TData>(string key, TData value)
        {
            mParamDic.TryGetValue(key, out var itemBase);
            var item = itemBase == null ? new Variable<TData>() : itemBase as Variable<TData>;
            if (item == null) return;
            item.Value = value;
            mParamDic[key] = item;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TData">泛型类型</typeparam>
        /// <returns></returns>
        public TData GetData<TData>(string key)
        {
            mParamDic.TryGetValue(key, out var itemBase);
            if (itemBase is Variable<TData> item) return item.Value;
            return default;
        }

        /// <summary>
        /// 关闭状态机
        /// </summary>
        public override void ShutDown()
        {
            mCurrState?.OnLeave();

            foreach (var state in mStateDic)
            {
                state.Value.OnDestroy();
            }

            mStateDic.Clear();
            mParamDic.Clear();
        }
    }
}