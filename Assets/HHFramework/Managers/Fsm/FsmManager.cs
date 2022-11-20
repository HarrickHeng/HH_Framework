using System;
using System.Collections.Generic;

namespace HHFramework
{
    /// <summary>
    /// 状态机管理器
    /// </summary>
    public class FsmManager : ManagerBase, IDisposable
    {
        /// <summary>
        /// 状态机字典
        /// </summary>
        private readonly Dictionary<int, FsmBase> mFsmDic;

        public FsmManager()
        {
            mFsmDic = new Dictionary<int, FsmBase>();
        }

        #region Create 创建状态机

        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <param name="fsmId">状态机编号</param>
        /// <param name="owner">拥有者</param>
        /// <param name="states">状态数组</param>
        /// <typeparam name="T">拥有者类型</typeparam>
        /// <returns></returns>
        public Fsm<T> Create<T>(int fsmId, T owner, FsmState<T>[] states) where T : class
        {
            var fsm = new Fsm<T>(fsmId, owner, states);
            mFsmDic[fsmId] = fsm;
            return fsm;
        }

        #endregion

        #region DestroyFsm 销毁状态机

        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <param name="fsmId"></param>
        public void DestroyFsm(int fsmId)
        {
            if (!mFsmDic.TryGetValue(fsmId, out var fsm)) return;
            fsm.ShutDown();
            mFsmDic.Remove(fsmId);
        }

        #endregion

        public void Dispose()
        {
            var enumerator = mFsmDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.ShutDown();
            }
            mFsmDic.Clear();
        }
    }
}