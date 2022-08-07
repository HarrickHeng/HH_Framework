using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色有限状态机管理器
/// </summary>
public class RoleFSMMgr
{
    /// <summary>
    /// 当前角色控制器
    /// </summary>
    public RoleCtrl CurrRoleCtrl { get; private set; }

    /// <summary>
    /// 当前角色状态枚举
    /// </summary>
    public ERoleState CurrRoleStateEnum { get; private set; }

    /// <summary>
    /// 当前角色状态
    /// </summary>
    private RoleStateAbstract m_CurrRoleState = null;

    /// <summary>
    /// 角色状态字典
    /// </summary>
    private Dictionary<ERoleState, RoleStateAbstract> m_RoleStateDic;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currRoleCtrl"></param>
    public RoleFSMMgr(RoleCtrl currRoleCtrl)
    {
        CurrRoleCtrl = currRoleCtrl;
        m_RoleStateDic = new Dictionary<ERoleState, RoleStateAbstract>();
        m_RoleStateDic[ERoleState.Idle01] = new RoleStateIdle(this);
        m_RoleStateDic[ERoleState.Attack] = new RoleStateAttack(this);
        m_RoleStateDic[ERoleState.Run01] = new RoleStateRun(this);
        m_RoleStateDic[ERoleState.Walk01] = new RoleStateWalk(this);
        m_RoleStateDic[ERoleState.Dead01] = new RoleStateDead(this);
        m_RoleStateDic[ERoleState.Hurt01] = new RoleStateHurt(this);
        m_RoleStateDic[ERoleState.Jump01] = new RoleStateJump(this);

        if (m_RoleStateDic.ContainsKey(CurrRoleStateEnum))
        {
            m_CurrRoleState = m_RoleStateDic[CurrRoleStateEnum];
        }
    }

    /// <summary>
    /// 每帧执行
    /// </summary>
    public void OnUpdate()
    {
        if (m_CurrRoleState != null)
        {
            m_CurrRoleState.OnUpdate();
        }
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="newERoleState">新状态</param>
    public void ChangeState(ERoleState newERoleState)
    {
        if (CurrRoleStateEnum == newERoleState)
            return;

        if (m_CurrRoleState != null)
            m_CurrRoleState.OnLeave();

        CurrRoleStateEnum = newERoleState;
        m_CurrRoleState = m_RoleStateDic[newERoleState];

        m_CurrRoleState.OnEnter();
    }
}
