using System;
using System.Collections.Generic;

namespace HHFramework
{
    /// <summary>
    /// 流程状态
    /// </summary>
    public enum ProcedureState
    {
        Launch = 0,
        CheckVersion = 1,
        Preload = 2,
        ChangeScene = 3,
        LogOn = 4,
        SelectRole = 5,
        EnterGame = 6,
        WorldMap = 7,
        GameLevel = 8,
    }

    /// <summary>
    /// 流程管理器
    /// </summary>
    public class ProcedureManager : ManagerBase, IDisposable
    {
        /// <summary>
        /// 流程状态机
        /// </summary>
        public Fsm<ProcedureManager> CurrFsm { get; private set; }

        /// <summary>
        /// 当前流程状态
        /// </summary>
        public ProcedureState CurrProcedureState => (ProcedureState)CurrFsm.CurrStateType;

        /// <summary>
        /// 当前的流程
        /// </summary>
        public FsmState<ProcedureManager> CurrProcedure => CurrFsm.GetState(CurrFsm.CurrStateType);

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            var status = new FsmState<ProcedureManager>[9];
            status[0] = new ProcedureLaunch();
            status[1] = new ProcedureCheckVersion();
            status[2] = new ProcedurePreload();
            status[3] = new ProcedureChangeScene();
            status[4] = new ProcedureLogOn();
            status[5] = new ProcedureSelectRole();
            status[6] = new ProcedureEnterGame();
            status[7] = new ProcedureWorldMap();
            status[8] = new ProcedureGameLevel();

            CurrFsm = GameEntry.Fsm.Create(this, status);
        }

        /// <summary>
        /// 切换流程状态
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(ProcedureState state)
        {
            CurrFsm.ChangeState((byte)state);
        }

        public void OnUpdate()
        {
            CurrFsm.OnUpdate();
        }

        public void Dispose()
        {
        }
    }
}