using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIConcertEditor : BaseUI
{
    public GameObject NPCContentDrawer;
    public GameObject NPCListDrawer;
    public GameObject NPCList;
    public GameObject NPCPrefab;
    private bool _isLoaded;
    private bool _isLoadedList;

    public SettingData _settingData;
    protected override void Awake()
    {
        base.Awake();
        _isLoaded = false;
        _isLoadedList = false;
        _settingData = Resources.Load<SettingData>("Scriptable Objects/Setting Data");
    }


    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        if (!_isLoaded)
        {
            int childCount = NPCContentDrawer.transform.childCount;
            var NPCs = _settingData.NPC;
            for (int i = 0; i < childCount; i++)
            {
                var index = i;
                Transform child = NPCContentDrawer.transform.GetChild(i);
                child.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log(index);
                    PlaySoundOnClick();
                    ShowCharacterList(index);
                });
                child.gameObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = NPCs[i].Split("/")[2];
            }
            _isLoaded = true;
        }
    }

    public void ShowCharacterList(int index)
    {
        NPCList.SetActive(true);
        var i = index;
        var character = Resources.Load<CharacterData>("Scriptable Objects/Character Data");
        var NPCs = _settingData.NPC;
        foreach (var npc in character.ListCharacter)
        {
            var npcButton = Instantiate(NPCPrefab, NPCListDrawer.transform, false);
            npcButton.GetComponentInChildren<TextMeshProUGUI>().text = npc.ModelName;
            npcButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlaySoundOnClick();
                NPCs[i] = character.ModelPath + npc.Path + npc.ModelName;
                NPCList.SetActive(false);
                UpdateNPCList();
            });
        }
        _isLoadedList = true;
    }
    public void UpdateNPCList()
    {
        int childCount = NPCContentDrawer.transform.childCount;
        var NPCs = _settingData.NPC;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = NPCContentDrawer.transform.GetChild(i);
            child.gameObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = NPCs[i].Split("/")[2];
        }

        foreach (Transform child in NPCListDrawer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnBackClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UINavigationTab);
        UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
    }
}
