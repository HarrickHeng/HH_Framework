using UnityEngine;

/// <summary>
/// 窗口UI基类
/// </summary>
public class UIWinBase : UIBase
{
    /// <summary>
    /// 挂点类型
    /// </summary>
    [Header("挂点类型")]
    public WinUIContainerType containerType = WinUIContainerType.Center;

    /// <summary>
    /// 打开方式
    /// </summary>
    [Header("窗口打开方式")]
    public WinShowStyle showStyle = WinShowStyle.Normal;

    /// <summary>
    /// 打开或关闭效果持续时间(秒)
    /// </summary>
    [Header("动画效果持续时间(秒)")]
    public float duration = 0.2f;

    /// <summary>
    /// 当前窗口类型
    /// </summary>
    [HideInInspector]
    public WinUIType CurrentUIType;

    /// <summary>
    /// 下一个要打开的窗口
    /// </summary>
    protected WinUIType NextUIType = WinUIType.None;

    protected virtual void Close()
    {
        WinUIMgr.Instance.CloseWindow(CurrentUIType);
    }

    /// <summary>
    /// 销毁之前执行
    /// </summary>
    public override void OnBeforeDispose()
    {
        LayerMgr.Instance.CheckOpenWindow();
        if (NextUIType == WinUIType.None)
            return;
        WinUIMgr.Instance.OpenWindow(NextUIType);
    }
}
