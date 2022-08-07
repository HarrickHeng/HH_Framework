using UnityEngine.UI;

/// <summary>
/// Test窗口UI控制器
/// </summary>
public class TestUIPanel : UIWinBase
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
            NextUIType = WinUIType.Test2;
        }
    }

#region UI_AUTOCODE_RC 8af3eb5293eb4526b5459d299dc60dfa

    private Button closeButton;

    public void CacheReference()
    {
        var rc = this.gameObject.GetComponent<ReferenceCtrl>();
        this.closeButton = rc.GetReference<Button>(0); // name: CloseButton
    }

#endregion

}
