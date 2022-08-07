using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 窗口UI管理器
/// </summary>
public class WinUIMgr : Singleton<WinUIMgr>
{
    private Dictionary<WinUIType, UIWinBase> m_DicWindow = new Dictionary<WinUIType, UIWinBase>();

    /// <summary>
    /// 已经打开的窗口数量
    /// </summary>
    /// <returns></returns>
    public int OpenWindowCount
    {
        get { return m_DicWindow.Count; }
    }

    #region OpenWindow 打开窗口
    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="type">窗口UI类型</param>
    /// <returns></returns>
    public GameObject OpenWindow(WinUIType type)
    {
        if (type == WinUIType.None)
        {
            return null;
        }

        GameObject obj = null;

        if (!m_DicWindow.ContainsKey(type))
        {
            //层级名称要与预设名称对应
            obj = ResourceMgr.Instance.Load(
                EResType.UIWin,
                string.Format("{0}UIPanel", type.ToString()),
                cache: true
            );

            UIWinBase winBase = obj.GetComponent<UIWinBase>();
            if (winBase == null)
            {
                Debug.LogError(string.Format("{0}UIPanel 预设体未附加脚本！", type.ToString()));
                return null;
            }

            m_DicWindow.Add(type, winBase);

            winBase.CurrentUIType = type;

            Transform transParent = null;
            switch (winBase.containerType)
            {
                case WinUIContainerType.Center:
                    transParent = SceneUIMgr.Instance.CurrentUIScene.Container_Center;
                    break;
            }

            obj.transform.SetParent(transParent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            obj.SetActive(false);
            StartShowWin(winBase, true);
        }
        else
        {
            obj = m_DicWindow[type].gameObject;
        }

        if (obj == null)
        {
            Debug.LogError(string.Format("{0}UIPanel 预设体不存在！", type.ToString()));
            return null;
        }

        //层级管理
        LayerMgr.Instance.SetLayer(obj);

        return obj;
    }
    #endregion

    #region CloseWindow 关闭窗口
    public void CloseWindow(WinUIType type)
    {
        if (m_DicWindow.ContainsKey(type))
        {
            StartShowWin(m_DicWindow[type], false);
        }
    }
    #endregion

    /// <summary>
    /// 销毁窗口
    /// </summary>
    /// <param name="winBase"></param>
    private void DestoryWin(UIWinBase winBase)
    {
        m_DicWindow.Remove(winBase.CurrentUIType);
        Object.Destroy(winBase.gameObject);
    }

    #region StartShowWin 开始打开窗口
    /// <summary>
    /// 开始打开窗口
    /// </summary>
    /// <param name="winBase"></param>
    /// <param name="isOpen">是否打开</param>
    private void StartShowWin(UIWinBase winBase, bool isOpen)
    {
        switch (winBase.showStyle)
        {
            case WinShowStyle.Normal:
                ShowNormal(winBase, isOpen);
                break;
            case WinShowStyle.CenterToBig:
                ShowCenterToBig(winBase, isOpen);
                break;
            case WinShowStyle.FromTop:
                ShowFromDir(winBase, 0, isOpen);
                break;
            case WinShowStyle.FromDown:
                ShowFromDir(winBase, 1, isOpen);
                break;
            case WinShowStyle.FromLeft:
                ShowFromDir(winBase, 2, isOpen);
                break;
            case WinShowStyle.FromRight:
                ShowFromDir(winBase, 3, isOpen);
                break;
        }
    }
    #endregion

    #region 各种显示效果
    /// <summary>
    /// 正常显示
    /// </summary>
    private void ShowNormal(UIWinBase winBase, bool isOpen)
    {
        if (isOpen)
        {
            winBase.gameObject.SetActive(true);
        }
        else
        {
            DestoryWin(winBase);
        }
    }

    /// <summary>
    /// 从中间放大
    /// </summary>
    private void ShowCenterToBig(UIWinBase winBase, bool isOpen)
    {
        Vector3 from = isOpen ? Vector3.zero : Vector3.one;
        Vector3 to = isOpen ? Vector3.one : Vector3.zero;

        winBase.gameObject.transform.localScale = from;
        winBase.gameObject.transform
            .DOScale(to, winBase.duration)
            .SetEase(GameMgr.Instance.UIAnimationCurve)
            .OnComplete(
                () =>
                {
                    if (!isOpen)
                    {
                        DestoryWin(winBase);
                    }
                }
            );

        winBase.gameObject.SetActive(true);
    }

    /// <summary>
    /// 从不同的方向加载窗口
    /// </summary>
    /// <param name="winBase"></param>
    /// <param name="dirType">0=从上 1=从下 2=从左 3=从右</param>
    /// <param name="isOpen"></param>
    private void ShowFromDir(UIWinBase winBase, int dirType, bool isOpen)
    {
        Vector3 from = Vector3.zero;
        Vector3 to = Vector3.zero;

        switch (dirType)
        {
            case 0:
                from = new Vector3(0, 1000, 0);
                break;
            case 1:
                from = new Vector3(0, -1000, 0);
                break;
            case 2:
                from = new Vector3(-1400, 0, 0);
                break;
            case 3:
                from = new Vector3(1400, 0, 0);
                break;
        }

        if (!isOpen)
        {
            Vector3 temp = from;
            from = to;
            to = temp;
        }

        winBase.gameObject.transform.localPosition = from;
        winBase.gameObject.transform
            .DOMove(to, winBase.duration)
            .SetEase(GameMgr.Instance.UIAnimationCurve)
            .OnComplete(
                () =>
                {
                    if (!isOpen)
                    {
                        DestoryWin(winBase);
                    }
                }
            );

        winBase.gameObject.SetActive(true);
    }
    #endregion

}
