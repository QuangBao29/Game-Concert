using UnityEngine;
using System.Collections.Generic;
using System;
using UI;

public class UIManager : PersistentManager<UIManager>
{
    private Dictionary<UIIndex, BaseUI> _uiDictionary;
    [SerializeField] private List<BaseUI> listUI = new();

    public UIIndex currentUIIndex;

    public override void Awake()
    {
        base.Awake();
        _uiDictionary = new Dictionary<UIIndex, BaseUI>();
        Application.quitting += () =>
        {
            if (_uiDictionary != null)
            {
                foreach (var item in _uiDictionary)
                {
                    var disposable = item.Value as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }

                _uiDictionary.Clear();
                _uiDictionary = null;
            }
        };
    }

    private void Start()
    {
        currentUIIndex = UIIndex.None;
        InitUI(null);
    }

    private void InitUI(Action callback)
    {
        foreach (var ui in listUI)
        {
            var goUI = ui.gameObject;
            goUI.transform.SetParent(transform, false);

            var baseUI = goUI.GetComponent<BaseUI>();
            baseUI.OnInit();

            _uiDictionary.Add(baseUI.index, baseUI);

            goUI.SetActive(false);
        }

        callback?.Invoke();

        var isUILoaded = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Main UI").isLoaded;
        ShowUI(isUILoaded ? UIIndex.UIAuthentication : UIIndex.UIHud);
    }

    public void ShowUI(UIIndex uiIndex, UIParam param = null, Action callback = null)
    {
        var ui = GetUI(uiIndex);
        ui.ShowUI(param, callback);

        if (uiIndex != UIIndex.UINavigationTab)
        {
            currentUIIndex = uiIndex;
        }
    }

    public void HideUI(BaseUI baseUI, Action callback = null)
    {
        baseUI.HideUI(callback);
        if (baseUI.index != UIIndex.UINavigationTab)
        {
            currentUIIndex = UIIndex.None;
        }
    }

    public void HideUI(UIIndex uiIndex, Action callback = null)
    {
        var ui = GetUI(uiIndex);
        ui.HideUI(callback);
        if (uiIndex != UIIndex.UINavigationTab)
        {
            currentUIIndex = UIIndex.None;
        }
    }


    private BaseUI GetUI(UIIndex uiIndex)
    {
        return _uiDictionary[uiIndex];
    }
}