using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{
    public SceneType CurrentSceneType { get; private set; }

    /// <summary>
    /// 加载开始界面
    /// </summary>
    public void LoadStart()
    {
        CurrentSceneType = SceneType.Start;
        SceneManager.LoadScene("Start");
    }
}
