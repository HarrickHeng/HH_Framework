using UnityEngine;

/// <summary>
/// 场景UI管理器
/// </summary>
public class SceneUIMgr : Singleton<SceneUIMgr>
{
    /// <summary>
    /// 当前场景UI
    /// </summary>
    public UISceneBase CurrentUIScene;

    #region 加载场景UI
    /// <summary>
    /// 加载场景UI
    /// </summary>
    /// <param name="type">场景UI类型</param>
    /// <returns></returns>
    public GameObject LoadSceneUI(SceneUIType type)
    {
        GameObject obj = null;
        switch (type)
        {
            case SceneUIType.Test:
                obj = ResourceMgr.Instance.Load(EResType.UIScene, "TestUIScene", cache: true);
                CurrentUIScene = obj.GetComponent<TestUIScene>();
                break;
        }
        return obj;
    }
    #endregion

    #region 场景UI类型
    public enum SceneUIType
    {
        Test
    }
    #endregion

}
