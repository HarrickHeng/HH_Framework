using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// Test场景UI控制器
/// </summary>
public class TestUIScene : UISceneBase
{
    protected override void OnStart()
    {
        base.OnStart();
        UniTask load = LoadView();
    }

    private async UniTask LoadView()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        GameObject obj = WinUIMgr.Instance.OpenWindow(WinUIType.Test);
    }
}
