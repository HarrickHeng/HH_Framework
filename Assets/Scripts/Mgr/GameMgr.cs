using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameMgr : MonoSingleton<GameMgr>
{
    static DisposableCtrl _DisposableCtrl;
    public static DisposableCtrl DisposableCtrl =>
        _DisposableCtrl ?? (_DisposableCtrl = new DisposableCtrl());

    static List<IUpdateable> _Updateables;
    public static List<IUpdateable> Updateables =>
        _Updateables ?? (_Updateables = new List<IUpdateable>());

    /// <summary>
    /// UI使用的摄像机
    /// </summary>
    /// <value></value>
    public static Camera uiCamera { get; private set; }

    /// <summary>
    /// UI画布
    /// </summary>
    /// <value></value>
    public static Canvas uiCanvas { get; private set; }

    [SerializeField]
    public AnimationCurve UIAnimationCurve = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(1, 1)
    );

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.Find("EventSystem"));

        uiCamera = transform.Find("Camera").GetComponent<Camera>();
        uiCanvas = transform.GetComponent<Canvas>();

        transform.GetSiblingIndex();
    }

    public GameObject GetSceneLoadLayer()
    {
        Transform tf = transform.Find("SceneLoadLayer");
        if (tf == null)
        {
            tf = UIHelper.AddNewUIChild(gameObject, "SceneLoadLayer").transform;
        }
        return tf.gameObject;
    }

    public GameObject GetSpecialLayer()
    {
        Transform tf = transform.Find("SpecialLayer");
        if (tf == null)
        {
            tf = UIHelper.AddNewUIChild(gameObject, "SpecialLayer").transform;
        }
        return tf.gameObject;
    }

    /// <summary>
    /// 判断所有UGUI
    /// </summary>
    /// <returns></returns>
    public bool IsTouchAllUI()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
#else
        if (Input.touchCount > 0)
        {
#endif
            PointerEventData eventData = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
            eventData.position = Input.mousePosition;
#else
            eventData.position = Input.GetTouch(0).position;
#endif
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            //向点击位置发射一条射线，检测是否点击UI
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults.Count > 0;
        }
        return false;
    }

    /// <summary>
    /// 只判断 MainCanvas UI层
    /// </summary>
    /// <returns></returns>
    public bool IsTouchMainUI()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
#else
        if (Input.touchCount > 0)
        {
#endif
            PointerEventData eventData = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
            eventData.position = Input.mousePosition;
#else
            eventData.position = Input.GetTouch(0).position;
#endif
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            //向点击位置发射一条射线，检测是否点击UI
            gameObject.GetComponent<GraphicRaycaster>().Raycast(eventData, raycastResults);
            return raycastResults.Count > 0;
        }
        return false;
    }

    /// <summary>
    /// 获取鼠标停留处UI (慎用)
    /// </summary>
    /// <param name="canvas"></param>
    /// <returns></returns>
    public GameObject GetOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster pr = gameObject.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        pr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            return results[0].gameObject;
        }
        return null;
    }

    public void Start()
    {
        //开始游戏
        GameObject obj = SceneUIMgr.Instance.LoadSceneUI(SceneUIMgr.SceneUIType.Test);
        obj.transform.SetParent(GameMgr.Instance.GetSceneLoadLayer().transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }

    public void OnApplicationQuit()
    {
        //游戏关闭
        SoundMgr.StopAllSound();
    }

    public void Update()
    {
        for (int i = 0; i < GameMgr.Updateables.Count; ++i)
        {
            GameMgr.Updateables[i].Update();
        }
    }
}
