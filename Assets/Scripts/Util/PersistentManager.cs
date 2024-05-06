using UnityEngine;

public class PersistentManager<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    private static bool applicationIsQuitting;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
                return null;

            if (!_instance)
            {
                Debug.LogError("Cannot find Object with type " + typeof(T));
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Already has instance of " + typeof(T));
            Destroy(this);
            return;
        }

        _instance = (T)(MonoBehaviour)this;

        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}