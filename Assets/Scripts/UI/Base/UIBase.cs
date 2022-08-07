using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour, IDisposable
{
    private int clickHashCode = 0;

    void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        ReferenceCtrlHelper.CacheReferenceHandle(this);
        //注册按钮事件
        Button[] btnArr = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btnArr.Length; ++i)
        {
            UIEventListener
                .Get(btnArr[i].gameObject)
                .AddListener(UIEventListener.UIClickEventType.Click, BtnClick);
        }
        OnStart();
    }

    void OnDestroy()
    {
        OnBeforeDispose();
        Dispose();
    }

    void BtnClick(PointerEventData eventData)
    {
        clickHashCode = eventData.pointerClick.GetHashCode();
        OnBtnClick(eventData); //执行有参数的重载函数
        OnBtnClick(); //执行无参数的重载函数
    }

    protected bool IsClick(Button btn)
    {
        return btn.gameObject.GetHashCode() == clickHashCode;
    }

    protected virtual void OnAwake() { }

    protected virtual void OnStart() { }

    protected virtual void OnBtnClick(PointerEventData eventData) { }

    protected virtual void OnBtnClick() { }

    protected virtual void OnBeforeDestroy() { }

    public virtual void OnBeforeDispose() { }

    public virtual void Dispose() { }
}
