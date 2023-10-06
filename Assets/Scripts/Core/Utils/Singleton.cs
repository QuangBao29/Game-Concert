using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}