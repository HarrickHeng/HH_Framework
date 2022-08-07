using UnityEngine;

public static class UIHelper
{
	#region 创建对象与销毁对象
    /// <summary>
    /// 把子类添加到父类中  并重置坐标
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    public static GameObject AddChild(GameObject parent, GameObject child)
    {
        if (parent == null || child == null)
        {
            return child;
        }
        RedirectParentObject(parent, child);
        return child;
    }

    /// <summary>
    /// 添加子对象，加载并创建
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public static GameObject AddChildAndLoad(GameObject parent, string assetName)
    {
        GameObject prefab = ResourceMgr.Instance.Load(EResType.Other, assetName, cache: true);
        
        if (prefab == null)
        {
            Debug.Log("load resources fail! assetName:" + assetName);
            return null;
        }
        return UIHelper.AddChildAndInstantiate(parent, prefab);
    }

    /// <summary>
    /// 添加子对象，并创建
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <returns></returns>
    public static GameObject AddChildAndInstantiate(GameObject parent, GameObject prefab)
    {
        if (parent == null)
        {
            return GameObject.Instantiate(prefab);
        }
        return UIHelper.AddChild(parent, GameObject.Instantiate(prefab));
    }

    /// <summary>
    /// 创建一个新的对象
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject AddNewChild(GameObject parent, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return new GameObject();
        }
        else
        {
            return UIHelper.AddChild(parent, new GameObject(name));
        }
    }

    public static GameObject AddNewUIChild(GameObject parent, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return new GameObject();
        }
        else
        {
            GameObject go = UIHelper.AddChild(parent, new GameObject(name));
            return go.AddComponent<RectTransform>().gameObject;
        }
    }

    /// <summary>
    /// 销毁所有子对象
    /// </summary>
    /// <param name="parent"></param>
    public static void DestroyChildren(GameObject parent)
    {
        if (parent != null)
        {
            bool isPlaying = Application.isPlaying;

            while (parent.gameObject.transform.childCount != 0)
            {
                Transform child = parent.gameObject.transform.GetChild(0);

                if (isPlaying)
                {
                    child.parent = null;
                    Object.Destroy(child.gameObject);
                }
                else
                    Object.DestroyImmediate(child.gameObject);
            }
        }
    }
	#endregion

    /// <summary>
    /// 执行该对象与子对象所有实现 IDisposable 接口的 Dispose方法
    /// </summary>
    /// <param name="go"></param>
    public static void ComponentsInChildrenIDisposable(GameObject go)
    {
        if (go == null)
            return;

        var arr = go.GetComponentsInChildren<IDisposable>();
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].Dispose();
        }
    }

    /// <summary>
    /// 重定向父对象
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void RedirectParentObject(GameObject parentGo, GameObject childrenGo)
    {
        if (childrenGo == null)
            return;
        if (parentGo == null)
            return;

        childrenGo.transform.SetParent(parentGo.transform, false);

        childrenGo.transform.localPosition = Vector3.zero;
        childrenGo.transform.localEulerAngles = Vector3.zero;
        childrenGo.transform.localScale = Vector3.one;

        var translist = childrenGo.GetComponentsInChildren<Transform>();
        for (int i = 0; i < translist.Length; ++i)
        {
            translist[i].gameObject.layer = parentGo.layer;
        }
    }

    public static Transform GetTransform(UnityEngine.Object obj)
    {
        if (obj == null)
        {
            Debug.Log("UIHelper transform 为空!");
            return null;
        }
        Transform tf = null;
        if (obj is Component)
        {
            tf = (obj as Component).transform;
        }
        else if (obj is GameObject)
        {
            tf = (obj as GameObject).transform;
        }
        else if (obj is Transform)
        {
            tf = (obj as Transform);
        }
        if (tf == null)
        {
            Debug.LogError("CS.UIHelper transform 收到非法类型：" + obj);
        }
        return tf;
    }

    public static Vector2 ScreenPointToLocalPointInRectangle(
        RectTransform rec,
        Vector2 pos,
        Camera cam
    )
    {
        Vector2 outpos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rec, pos, cam, out outpos))
        {
            return outpos;
        }
        return outpos;
    }

    public static bool ForceRebuildLayoutImmediate(RectTransform res)
    {
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(res);
        return true;
    }

    // [LuaInterface.NoToLua]
    public static T FindAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var com = gameObject.GetComponent<T>();
        if (com != null)
        {
            return com;
        }
        else
        {
            return gameObject.AddComponent<T>();
        }
    }

    // [LuaInterface.NoToLua]
    public static T FindAddComponent<T>(this Transform transform) where T : Component
    {
        var com = transform.GetComponent<T>();
        if (com != null)
        {
            return com;
        }
        else
        {
            return transform.gameObject.AddComponent<T>();
        }
    }

    /// <summary>
    /// 灰化对象
    /// </summary>
    /// <param name="grayscale">灰化  true：灰化，false：原状态</param>
    /// <param name="disableEvent">禁止点击</param>
    public static void UIGrayscale(UnityEngine.Object obj, bool grayscale, bool disableEvent)
    {
        var tf = GetTransform(obj);
        // var uiGrayscale = tf.gameObject.FindAddComponent<UGUIExtend.UIGrayscale>();
        // uiGrayscale.Grayscale(grayscale);
        // uiGrayscale.DisableEvent(disableEvent);
    }
}
