using System;
using System.Collections.Generic;

namespace HHFramework
{
    /// <summary>
    /// 流程组件
    /// </summary>
    public class ProcedureComponent : HHBaseComponent, IUpdateComponent
    {
        private ProcedureManager mProcedureManager;

        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);
            mProcedureManager = new ProcedureManager();
        }

        /// <summary>
        /// 当前流程状态
        /// </summary>
        public ProcedureState CurrProcedureState => mProcedureManager.CurrProcedureState;

        /// <summary>
        /// 当前的流程
        /// </summary>
        public FsmState<ProcedureManager> CurrProcedure => mProcedureManager.CurrProcedure;

        /// <summary>
        /// 切换流程状态
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(ProcedureState state)
        {
            mProcedureManager.ChangeState(state);
        }

        protected override void OnStart()
        {
            base.OnStart();

            // 在Start初始化 是为防止有些组件未准备完成
            mProcedureManager.Init();
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TData">泛型类型</typeparam>
        public void SetData<TData>(string key, TData value)
        {
            mProcedureManager.CurrFsm.SetData(key, value);
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TData">泛型类型</typeparam>
        /// <returns></returns>
        public TData GetData<TData>(string key)
        {
            return mProcedureManager.CurrFsm.GetData<TData>(key);
        }

        public override void ShutDown()
        {
            mProcedureManager.Dispose();
        }

        public void OnUpdate()
        {
            mProcedureManager.OnUpdate();
        }
    }
}