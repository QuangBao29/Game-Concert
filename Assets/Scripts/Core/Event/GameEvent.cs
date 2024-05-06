using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/Game Event")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new();

    public void Invoke(Component sender, object data)
    {
        foreach (var listener in listeners)
        {
            listener.OnEventInvoke(sender, data);
        }
    }

    public void Register(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void Deregister(GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}

[Serializable]
public class ExtGameEvent : UnityEvent<Component, object>
{
}