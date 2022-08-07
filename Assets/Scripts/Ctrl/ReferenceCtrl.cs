using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ReferenceCtrlData
{
    public Object gameObject;
    public string key;
}

public class ReferenceCtrlDataComparer : IComparer<ReferenceCtrlData>
{
    public int Compare(ReferenceCtrlData x, ReferenceCtrlData y)
    {
        return string.Compare(x.key, y.key, StringComparison.Ordinal);
    }
}

public class ReferenceCtrl : MonoBehaviour, ISerializationCallbackReceiver
{
    public static UnCheckDictionary<GameObject, ReferenceCtrl> allReferenceCtrlCaches =
        new UnCheckDictionary<GameObject, ReferenceCtrl>();

    public List<ReferenceCtrlData> data = new List<ReferenceCtrlData>();

    [SerializeField]
    string uuid;

    public string UUID
    {
        get
        {
            if (string.IsNullOrEmpty(this.uuid))
                this.uuid = Guid.NewGuid().ToString();
            return this.uuid;
        }
    }

    public string RefreshUUID()
    {
        this.uuid = Guid.NewGuid().ToString();
        return this.uuid;
    }

    public void AddOrReplace(string key, object obj)
    {
        throw new NotImplementedException();
    }

    public static ReferenceCtrl Get(GameObject gameObject)
    {
        return allReferenceCtrlCaches[gameObject];
    }

    public T Get<T>(string key, bool nullTips = true) where T : class
    {
        object obj = this.GetObject(key);
        if (obj == null)
        {
            if (nullTips)
                Debug.LogError("Can not find key: " + key);
            return null;
        }

        if (!(obj is T))
        {
            if (nullTips)
                Debug.LogError("Key: " + key + " is not type: " + typeof(T));
            return null;
        }

        return obj as T;
    }

    public T Get<T>(int index) where T : class
    {
        object obj = this.GetObject(index);
        if (obj == null)
        {
            Debug.LogError("Can not find index: " + index);
            return null;
        }

        if (!(obj is T))
        {
            Debug.LogError("Index: " + index + " is not type: " + typeof(T) + $", {obj}");
            return null;
        }

        return obj as T;
    }

    public object GetObject(string key)
    {
        foreach (var e in this.data)
        {
            if (key.Equals(e.key))
                return e.gameObject;
        }

        return null;
    }

    public object GetObject(int index)
    {
        if (index < 0 || index >= this.data.Count)
            return null;
        return this.data[index].gameObject;
    }

    public T GetReference<T>(string key, bool nullTips = true) where T : class
    {
        return this.Get<T>(key, nullTips);
    }

    public T GetReference<T>(int index) where T : class
    {
        return this.Get<T>(index);
    }

    void Awake()
    {
        allReferenceCtrlCaches.Add(this.gameObject, this);
    }

    void OnDestroy()
    {
        allReferenceCtrlCaches.Remove(this.gameObject);
        this.data.Clear();
        this.data = null;
    }

    public static void Debug_DumpAll()
    {
        Debug.Log($"RC COUNT: {allReferenceCtrlCaches.Count}");
        foreach (var a in allReferenceCtrlCaches)
        {
            var path = PathHelper.GameObjectPath(a.Key);
            Debug.Log($"RC: {path} => {a.Value.data.Count}");
        }
    }

#if UNITY_EDITOR

    public void Add(string key, Object obj)
    {
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty dataProperty = serializedObject.FindProperty("data");
        int i;
        for (i = 0; i < data.Count; i++)
        {
            if (data[i].key == key)
            {
                break;
            }
        }
        if (i != data.Count)
        {
            SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
        }
        else
        {
            dataProperty.InsertArrayElementAtIndex(i);
            SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("key").stringValue = key;
            element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
        }
        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    public void Remove(string key)
    {
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty dataProperty = serializedObject.FindProperty("data");
        int i;
        for (i = 0; i < this.data.Count; i++)
        {
            if (this.data[i].key == key)
            {
                break;
            }
        }

        if (i != this.data.Count)
        {
            dataProperty.DeleteArrayElementAtIndex(i);
        }

        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    public void Sort()
    {
        SerializedObject serializedObject = new SerializedObject(this);
        this.data.Sort(new ReferenceCtrlDataComparer());
        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }
#endif
    //在序列化后运行
    public void OnBeforeSerialize() { }

    //在反序列化后运行
    public void OnAfterDeserialize() { }
}
