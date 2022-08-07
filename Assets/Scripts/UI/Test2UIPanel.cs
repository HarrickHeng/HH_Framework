using UnityEngine.UI;

/// <summary>
/// Test2窗口UI控制器
/// </summary>
public class Test2UIPanel : UIWinBase
{
    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnBtnClick()
    {
        if (IsClick(closeButton))
        {
            Close();
            // NextUIType = WinUIType.Test;
        }
    }

#region UI_AUTOCODE_RC c2b714159dd9480ca2d2440a68436c88

private Button closeButton;

public void CacheReference()
{
    var rc = this.gameObject.GetComponent<ReferenceCtrl>();
    this.closeButton = rc.GetReference<Button>(0); // name: CloseButton
}

#endregion

}
