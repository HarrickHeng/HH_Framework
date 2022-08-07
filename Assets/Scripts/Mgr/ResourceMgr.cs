using System;
using System.Collections;
using System.Text;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 资源管理器
/// </summary>
public class ResourceMgr : Singleton<ResourceMgr>
{
    /// <summary>
    /// 预设缓存列表
    /// </summary>
    private Hashtable m_PrefabTable;

    public ResourceMgr()
    {
        m_PrefabTable = new Hashtable();
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();
        m_PrefabTable.Clear();
        //释放未使用的资源
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="type">资源类型</param>
    /// <param name="name">资源名称</param>
    /// <returns>路径</returns>
    private string GetPath(EResType type, string name)
    {
        StringBuilder sbr = new StringBuilder();
        switch (type)
        {
            case EResType.UIWin:
                sbr.Append("Prefabs/UIWin/");
                break;
            case EResType.UIScene:
                sbr.Append("Prefabs/UIScene/");
                break;
            case EResType.UIItem:
                sbr.Append("Prefabs/UIItem/");
                break;
            case EResType.Role:
                sbr.Append("Prefabs/Role/");
                break;
        }
        sbr.Append(name);

        return sbr.ToString();
    }

#region 异步资源加载
    /// <summary>
    /// 异步资源加载
    /// </summary>
    /// <param name="type">资源类型</param>
    /// <param name="name">资源名称</param>
    /// <param name="cache">是否加入缓存</param>
    /// <returns>预设克隆体</returns>
    public async UniTask LoadAsync(
        EResType type,
        string name,
        bool cache = false,
        Action<GameObject> callback = null
    )
    {
        GameObject obj = null;

        if (m_PrefabTable.Contains(name))
        {
            Debug.Log("从缓存中加载资源：" + name);
            obj = m_PrefabTable[name] as GameObject;
        }
        else
        {
            obj = await Resources.LoadAsync(GetPath(type, name)) as GameObject;
            if (cache)
                m_PrefabTable.Add(name, obj);
        }

        if (obj != null)
        {
            obj = GameObject.Instantiate(obj);
            callback?.Invoke(obj);
        }
        else
        {
            Debug.LogError("异步加载加载资源失败：" + name);
        }
    }
#endregion

#region 同步资源加载
    /// <summary>
    /// 同步资源加载
    /// </summary>
    /// <param name="type">资源类型</param>
    /// <param name="name">资源名称</param>
    /// <param name="cache">是否加入缓存</param>
    /// <returns>预设克隆体</returns>
    public GameObject Load(EResType type, string name, bool cache = false)
    {
        GameObject obj = null;
        if (m_PrefabTable.Contains(name))
        {
            Debug.Log("从缓存中加载资源：" + name);
            obj = m_PrefabTable[name] as GameObject;
        }
        else
        {
            obj = Resources.Load<GameObject>(GetPath(type, name));
            if (cache)
                m_PrefabTable.Add(name, obj);
        }

        return GameObject.Instantiate(obj);
    }
#endregion

}
