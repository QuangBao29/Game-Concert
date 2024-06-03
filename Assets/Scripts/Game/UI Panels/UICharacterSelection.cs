using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class UICharacterSelection : BaseUI
{
    public GameEvent onCharacterClick;

    private string _currentCharacterPath;

    private CharacterData _characterData;
    private GameObject _characterButtonPrefab;
    private Sprite _characterAvatar;

    public GameObject contentDrawer;
    private bool _isLoaded;

    protected override void Awake()
    {
        base.Awake();
        _characterData = Resources.Load<CharacterData>("Scriptable Objects/Character Data");
        _isLoaded = false;
    }

    private void OnCharacterClick()
    {
        PlaySoundOnClick();

        onCharacterClick.Invoke(this, new Dictionary<string, string>
        {
            { "Character Path", _currentCharacterPath }
        });
    }

    private void LoadCharacterList()
    {
        if (!_characterButtonPrefab)
        {
            _characterButtonPrefab = ResourceManager.LoadPrefabAsset("Assets/Prefabs/UI/Character Button.prefab");
        }

        foreach (var character in _characterData.ListCharacter)
        {
            if (character.Path == "Men/")
            {
                _characterAvatar = ResourceManager.LoadSprite("Assets/Art/Sprite/Icon/" + character.ModelName + "1.png");
            }
            else if (character.Path == "Women/")
            {
                _characterAvatar = ResourceManager.LoadSprite("Assets/Art/Sprite/Icon/" + character.ModelName + "0.png");
            }
            else
            {
                _characterAvatar = ResourceManager.LoadSprite("Assets/Art/Sprite/Icon/Avatar Default.png");
            }
            var characterButton = Instantiate(_characterButtonPrefab, contentDrawer.transform, false);
            characterButton.GetComponent<Image>().sprite = _characterAvatar;
            characterButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_currentCharacterPath == _characterData.ModelPath + character.Path + character.ModelName) return;
                _currentCharacterPath = _characterData.ModelPath + character.Path + character.ModelName;
                OnCharacterClick();
            });
        }
    }

    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        if (!_isLoaded)
        {
            LoadCharacterList();
            _isLoaded = true;
        }
    }
}