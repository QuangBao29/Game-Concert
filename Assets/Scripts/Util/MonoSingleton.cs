using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : Component
{
    private static T _singleton;

    public static T Instance
    {
        get
        {
            if (_singleton != null) return _singleton;

            _singleton = (T)FindObjectOfType(typeof(T));
            if (_singleton != null) return _singleton;

            var go = new GameObject
            {
                name = typeof(T).ToString()
            };
            _singleton = go.AddComponent<T>();

            return _singleton;
        }
    }
}