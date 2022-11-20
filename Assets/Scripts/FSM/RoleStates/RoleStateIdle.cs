using Common;

namespace FSM.RoleStates
{
    public class RoleStateIdle : RoleStateAbstract
    {
        public RoleStateIdle(RoleFSMMgr currRoleFSMMgr) : base(currRoleFSMMgr) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.speed = 1.0f;
            Animator.SetBool(ToAnimatorCondition.ToIdle.ToString(), true);
            Animator.SetInteger("CurrState", (int)ERoleState.Idle01);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CurrAnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            if (CurrAnimatorStateInfo.IsName(RoleAnimatorName.Idle01.ToString()))
            {
                Animator.SetInteger("CurrState", (int)ERoleState.Idle01);
            }
        }

        public override void OnLeave()
        {
            base.OnLeave();
            Animator.SetBool(ToAnimatorCondition.ToIdle.ToString(), false);
        }
    }
}
