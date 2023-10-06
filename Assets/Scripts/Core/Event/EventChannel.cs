using System.Collections.Generic;
using UnityEngine;

namespace Core.Event
{
    [CreateAssetMenu(menuName = "Event Channel")]
    public class EventChannel : ScriptableObject
    {
        public List<Listener> listeners = new List<Listener>();

        public void Raise(Component sender, object data)
        {
            foreach (var listener in listeners)
            {
                listener.OnEventRaised(sender, data);
            }
        }

        public void RegisterListener(Listener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void UnregisterListener(Listener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }
    }
}
