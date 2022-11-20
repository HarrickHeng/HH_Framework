using Common;

namespace FSM.RoleStates
{
    public class RoleStateAttack : RoleStateAbstract
    {
        public RoleStateAttack(RoleFSMMgr currRoleFSMMgr) : base(currRoleFSMMgr) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.speed = 1.0f;
            Animator.SetBool(ToAnimatorCondition.ToPyhAttack.ToString(), true);
            Animator.SetInteger("CurrState", (int)ERoleState.Attack);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CurrAnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            if (CurrAnimatorStateInfo.IsName(RoleAnimatorName.Attack01.ToString()))
            {
                Animator.SetInteger("CurrState", (int)ERoleState.Attack);
                Animator.SetInteger(ToAnimatorCondition.ToPyhAttack.ToString(), 1);
                if (CurrAnimatorStateInfo.normalizedTime >= 1)
                {
                    //reset()
                    Animator.SetBool(ToAnimatorCondition.ToIdle.ToString(), true);
                }
            }
            if (CurrAnimatorStateInfo.IsName(RoleAnimatorName.Attack02.ToString()))
            {
                Animator.SetInteger("CurrState", (int)ERoleState.Attack);
                Animator.SetInteger(ToAnimatorCondition.ToPyhAttack.ToString(), 2);
                if (CurrAnimatorStateInfo.normalizedTime >= 1)
                {
                    //reset()
                    Animator.SetBool(ToAnimatorCondition.ToIdle.ToString(), true);
                }
            }
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }
    }
}
