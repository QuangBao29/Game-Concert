using System;
using UnityEngine;
using UI;

public abstract class BaseUI : MonoBehaviour
{
    public UIIndex index;
    public GameEvent playSoundOnClick;
    protected RectTransform Transform;

    protected virtual void Awake()
    {
        Transform = GetComponent<RectTransform>();
    }

    public virtual void OnInit()
    {
    }

    protected virtual void OnSetup(UIParam param = null)
    {
    }

    protected virtual void OnShow(UIParam param = null)
    {
    }

    protected virtual void OnHide()
    {
    }

    public void ShowUI(UIParam param = null, Action callback = null)
    {
        gameObject.SetActive(true);
        OnSetup(param);
        OnShow(param);
        callback?.Invoke();
    }

    public void HideUI(Action callback = null)
    {
        OnHide();
        gameObject.SetActive(false);
        callback?.Invoke();
    }

    protected void PlaySoundOnClick()
    {
        playSoundOnClick.Invoke(this, 0);
    }
}