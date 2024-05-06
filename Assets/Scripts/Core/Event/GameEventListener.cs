using UnityEngine;

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public ExtGameEvent response;

    private void OnEnable()
    {
        gameEvent.Register(this);
    }

    private void OnDisable()
    {
        gameEvent.Deregister(this);
    }
    public void OnEventInvoke(Component sender, object data)
    {
        response?.Invoke(sender, data);
    }
    
}