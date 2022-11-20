using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T m_Instance = null;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType(typeof(T)) as T;
                if (m_Instance == null)
                {
                    Creat();
                }
            }

            return m_Instance;
        }
    }

    private static void Creat()
    {
        m_Instance = new GameObject(
            "Singleton of " + typeof(T),
            typeof(T)
        ).GetComponent<T>();
        m_Instance.Init();
    }

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
    }

    public virtual void Init()
    {
    }

    private void OnApplicationQuit()
    {
        m_Instance = null;
    }
}