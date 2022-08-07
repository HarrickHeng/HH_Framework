using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色信息基类
/// </summary>
public class RoleInfoBase
{
    /// <summary>
    /// 角色服务器编号
    /// </summary>
    public int RoleServerId;
    /// <summary>
    /// 角色编号
    /// </summary>
    public int RoleId;
    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName;
    /// <summary>
    /// 角色最大血量
    /// </summary>
    public int MaxHP;
    /// <summary>
    /// 角色当前血量
    /// </summary>
    public int CurrHP;
}
