using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIEventListener
    : UIBehaviour,
      IPointerDownHandler,
      IPointerUpHandler,
      IPointerClickHandler,
      IPointerEnterHandler,
      IPointerExitHandler,
      IDisposable,
      IBeginDragHandler,
      IDragHandler,
      IEndDragHandler
{
    public enum UIClickEventType
    {
        None,
        Up,
        Click,
        DoubleClick,
        Down,
        Press,
        Continue,
        BeginDrag,
        Drag,
        EndDrag,
        Exit,
        Enter,
    }

    [Serializable]
    public class ClickEventClass : UnityEvent<PointerEventData>
    {
        public ClickEventClass() { }
    }

    /// <summary>
    /// 声音名字
    /// </summary>
    [HideInInspector]
    public string soundId = "";

    /// <summary>
    /// 延时点击
    /// </summary>
    [HideInInspector]
    public float delayClickTime = 0.5f;

    /// <summary>
    /// 时间内双击有效
    /// </summary>
    [HideInInspector]
    public float doubleClickTime = 0.5f;

    /// <summary>
    /// 高准精度 (按下后如果鼠标发生了移动，则为无效点击)
    /// </summary>
    [HideInInspector]
    public bool highPrecision = true;

    /// <summary>
    /// 高精准度情况下，鼠标按下后拖动超过一定的距离
    /// </summary>
    [HideInInspector]
    public int highPrecisionDistance = 5;

    /// <summary>
    /// 低延时（按下后超过一定时间弹起，则为无效点击）
    /// </summary>
    [HideInInspector]
    public bool lowLatency = false;

    /// <summary>
    /// 低延时情况下，超过的时间(秒)
    /// </summary>
    [HideInInspector]
    public int lowLatencyTime = 1;

    /// <summary>
    /// 长按生效时间
    /// </summary>
    [HideInInspector]
    public float pressTime = 0.5f;

    private const float maxDoubleClickTime = 10;
    private const float maxcurrentPressTime = 1000;
    private float currentDoubleClickTime = -maxDoubleClickTime; //初始值为相反数，防止差值小于DoubleClickTime
    private float currentPressTime = maxcurrentPressTime * 10; //初始值设置要比最大值大，防止第一次差值大于PressTime
    private float currentUpTime = maxcurrentPressTime * 10; //当前弹起时间
    private float currentDelayClickTime = 0;

    private Vector3 lastDragVector2 = Vector2.zero; //上次拖动时的坐标
    private Vector3 dragDistanceVector2 = Vector2.zero; //从开始拖动到弹起时中间移动的总距离

    UIClickEventType currentClickEventType;
    PointerEventData currentPointerEventData;

    public static bool ExecuteEvent(GameObject go, int pEventType)
    {
        UIClickEventType eventType = (UIClickEventType)pEventType;
        switch (eventType)
        {
            case UIClickEventType.Up:
                return ExecuteEvents.Execute(
                    go,
                    new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerUpHandler
                );
            case UIClickEventType.Click:
                return ExecuteEvents.Execute(
                    go,
                    new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerClickHandler
                );
            case UIClickEventType.Down:
                return ExecuteEvents.Execute(
                    go,
                    new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerDownHandler
                );
            case UIClickEventType.BeginDrag:
                return ExecuteEvents.Execute(
                    go,
                    new PointerEventData(EventSystem.current),
                    ExecuteEvents.beginDragHandler
                );
            case UIClickEventType.Drag:
                return ExecuteEvents.Execute(
                    go,
                    new PointerEventData(EventSystem.current),
                    ExecuteEvents.dragHandler
                );
            case UIClickEventType.EndDrag:
                return ExecuteEvents.Execute(
                    go,
                    new PointerEventData(EventSystem.current),
                    ExecuteEvents.endDragHandler
                );
            case UIClickEventType.Exit:
                return ExecuteEvents.Execute(
                    go,
                    new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerExitHandler
                );
            case UIClickEventType.Enter:
                return ExecuteEvents.Execute(
                    go,
                    new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerEnterHandler
                );
            default:
                return false;
        }
    }

    public static UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null)
        {
            listener = go.AddComponent<UIEventListener>();
        }
        return listener;
    }

    private Dictionary<UIClickEventType, ClickEventClass> eventDic =
        new Dictionary<UIClickEventType, ClickEventClass>();

    private ScrollRect scrollRect;
    private UIEventListener scrollRectEventListener;

    protected override void Start()
    {
        this.OnEnable();
    }

    protected override void OnEnable()
    {
        scrollRect = transform.GetComponentInParent<ScrollRect>();
        if (scrollRect != null && scrollRect.gameObject != gameObject)
        {
            scrollRectEventListener = scrollRect.GetComponent<UIEventListener>();
        }
    }

    public UIEventListener AddListener(int listenerType, UnityAction<PointerEventData> call)
    {
        return this.AddListener((UIClickEventType)listenerType, call);
    }

    public UIEventListener AddListener(
        UIClickEventType listenerType,
        UnityAction<PointerEventData> call
    )
    {
        if (!eventDic.ContainsKey(listenerType))
        {
            eventDic.Add(listenerType, new ClickEventClass());
        }
        eventDic[listenerType].AddListener(call);
        return this;
    }

    public void RemoveListener(int listenerType, UnityAction<PointerEventData> call)
    {
        RemoveListener((UIClickEventType)listenerType, call);
    }

    public void RemoveListener(UIClickEventType listenerType, UnityAction<PointerEventData> call)
    {
        if (eventDic.ContainsKey(listenerType))
        {
            eventDic[listenerType].RemoveListener(call);
            eventDic[listenerType].Invoke(null);
        }
    }

    public void RemoveListener(int listenerType)
    {
        RemoveListener((UIClickEventType)listenerType);
    }

    public void RemoveListener(UIClickEventType listenerType)
    {
        if (eventDic.ContainsKey(listenerType))
        {
            eventDic[listenerType].RemoveAllListeners();
            eventDic[listenerType].Invoke(null);
        }
    }

    public void RemoveAllListener()
    {
        foreach (var kv in eventDic)
        {
            kv.Value.RemoveAllListeners();
            kv.Value.Invoke(null);
        }
        eventDic.Clear();
    }

    public virtual void OnBeforeDispose() { }

    public virtual void Dispose()
    {
        RemoveAllListener();
    }

	#region 事件接口类实现


    public void OnPointerClick(PointerEventData eventData)
    {
        if (highPrecision)
        {
            if (Vector2.Distance(Vector2.zero, dragDistanceVector2) > highPrecisionDistance)
            {
                return;
            }
        }
        if (lowLatency)
        {
            if (currentUpTime - currentPressTime > lowLatencyTime)
            {
                return;
            }
        }
        if (Time.realtimeSinceStartup - currentDelayClickTime >= delayClickTime)
        {
            OnEvent(UIClickEventType.Click, eventData);
            currentDelayClickTime = Time.realtimeSinceStartup;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentPointerEventData = eventData;
        OnEvent(UIClickEventType.Down, eventData);
        currentClickEventType = UIClickEventType.Down;

        currentPressTime = Time.realtimeSinceStartup;

        if (Time.realtimeSinceStartup - currentDoubleClickTime < doubleClickTime)
        {
            OnEvent(UIClickEventType.DoubleClick, eventData);
            currentDoubleClickTime = -maxDoubleClickTime;
        }
        else
        {
            currentDoubleClickTime = Time.realtimeSinceStartup;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        currentUpTime = Time.realtimeSinceStartup;
        currentClickEventType = UIClickEventType.Up;
        OnEvent(UIClickEventType.Up, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEvent(UIClickEventType.Enter, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnEvent(UIClickEventType.Exit, eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragVector2 = Input.mousePosition;

        OnEvent(UIClickEventType.BeginDrag, eventData);
        if (scrollRect)
            scrollRect.OnBeginDrag(eventData);
        if (scrollRectEventListener)
            scrollRectEventListener.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 moveDistance = Input.mousePosition - lastDragVector2;
        lastDragVector2 = Input.mousePosition;
        dragDistanceVector2 = new Vector2(
            Mathf.Abs(moveDistance.x) + dragDistanceVector2.x,
            Mathf.Abs(moveDistance.y) + dragDistanceVector2.y
        );

        OnEvent(UIClickEventType.Drag, eventData);
        if (scrollRect)
            scrollRect.OnDrag(eventData);
        if (scrollRectEventListener)
            scrollRectEventListener.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragDistanceVector2 = Vector2.zero;

        OnEvent(UIClickEventType.EndDrag, eventData);
        if (scrollRect)
            scrollRect.OnEndDrag(eventData);
        if (scrollRectEventListener)
            scrollRectEventListener.OnEndDrag(eventData);
    }

	#endregion 事件接口类实现

    private void OnEvent(UIClickEventType clickEventType, PointerEventData eventData)
    {
        if (!eventDic.ContainsKey(clickEventType))
        {
            return;
        }

        ClickEventClass eventClass = eventDic[clickEventType];
        switch (clickEventType)
        {
            case UIClickEventType.Click:
            {
                if (!string.IsNullOrEmpty(soundId))
                {
                    // SoundMgr.Instance.PlaySound("Sound/" + soundId);
                }
                break;
            }
        }
        eventClass.Invoke(eventData);
    }

    void Update()
    {
        if (currentClickEventType == UIClickEventType.Down)
        {
            OnEvent(UIClickEventType.Continue, currentPointerEventData);
            if (Time.realtimeSinceStartup - currentPressTime > pressTime)
            {
                OnEvent(UIClickEventType.Press, currentPointerEventData);
                currentPressTime = maxcurrentPressTime * 10;
            }
        }
    }
}
