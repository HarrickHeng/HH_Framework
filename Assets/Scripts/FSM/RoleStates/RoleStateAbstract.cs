using UnityEngine;

namespace FSM.RoleStates
{
    /// <summary>
    /// 角色状态抽象基类
    /// </summary>
    public class RoleStateAbstract
    {
        /// <summary>
        /// 当前角色有限状态机
        /// </summary>
        public RoleFSMMgr CurrRoleFSMMgr { get; private set; }

        public Animator Animator{ get; private set; }

        /// <summary>
        /// 当前动画状态信息
        /// </summary>
        public AnimatorStateInfo CurrAnimatorStateInfo { get; set; }

        public RoleStateAbstract(RoleFSMMgr currRoleFSMMgr)
        {
            CurrRoleFSMMgr = currRoleFSMMgr;
            Animator =  currRoleFSMMgr.CurrRoleCtrl.Animator;
        }

        /// <summary>
        /// 进入状态
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// 执行状态
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        ///  离开状态
        /// </summary>
        public virtual void OnLeave() { }
    }
}
