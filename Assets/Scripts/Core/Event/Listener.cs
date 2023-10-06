using Core.Event;
using UnityEngine;

public class Listener : MonoBehaviour
{
    public EventChannel eventChannel;
    public GameEvent response;

    private void OnEnable()
    {
        eventChannel.RegisterListener(this);
    }

    private void OnDisable()
    {
        eventChannel.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response?.Invoke(sender, data);
    }
}
