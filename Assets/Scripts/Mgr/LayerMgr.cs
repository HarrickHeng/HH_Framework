using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI层级管理器
/// </summary>
public class LayerMgr : Singleton<LayerMgr>
{
    /// <summary>
    /// 层级深度
    /// </summary>
    private int m_Depth = 50;

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
        m_Depth = 50;
    }

    /// <summary>
    /// 检查打开的窗口数量 为0时重置
    /// </summary>
    public void CheckOpenWindow()
    {
        if (WinUIMgr.Instance.OpenWindowCount == 0)
        {
            Reset();
        }
    }

    /// <summary>
    /// 设置层级
    /// </summary>
    /// <param name="obj"></param>
    public void SetLayer(GameObject obj)
    {
        m_Depth += 1;

        Canvas[] canvasArr = obj.GetComponentsInChildren<Canvas>();

        if (canvasArr.Length > 0)
        {
            for (int i = 0; i < canvasArr.Length; ++i)
            {
                Canvas canvas = canvasArr[i];
                canvas.overrideSorting = true;
                canvas.sortingOrder += m_Depth;
            }
        }
    }
}
