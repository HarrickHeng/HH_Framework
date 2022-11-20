#region 场景类型类型

namespace Common
{
    public enum SceneType
    {
        None,
        Start
    }

    #endregion

    #region 窗口UI类型

    public enum WinUIType
    {
        None,
        Test,
        Test2
    }

    #endregion

    #region 窗口UI显示位置

    public enum WinUIContainerType
    {
        TL, //左上
        TR, //右上
        BL, //左下
        BR, //右下
        Center //居中
    }

    #endregion

    #region 窗口UI显示方式

    public enum WinShowStyle
    {
        Normal, //常规
        CenterToBig, //从中间放大
        FromTop, //从上到下
        FromDown, //从下到上
        FromLeft, //从左到右
        FromRight //从右到左
    }

    #endregion

    #region 资源类型

    public enum EResType
    {
        UIScene,
        UIWin,
        UIItem,
        Role,
        Other
    }

    #endregion

    #region 角色类型

    public enum ERoleType
    {
        None, //未设置
        MainPlayer, //当前玩家
        Monster //怪物
    }

    #endregion

    #region 角色状态

    public enum ERoleState
    {
        None,
        Idle01,
        Attack,
        Run01,
        Walk01,
        Dead01,
        Hurt01,
        Jump01
    }

    #endregion

    public enum RoleAnimatorName
    {
        Idle01,
        Attack01,
        Attack02,
        Run01,
        Walk01,
        Dead01,
        Hurt01,
        Jump01
    }

    public enum ToAnimatorCondition
    {
        ToIdle,
        ToWalk,
        ToRun,
        ToPyhAttack,
        ToJump,
        ToHurt,
        ToDie
    }
}