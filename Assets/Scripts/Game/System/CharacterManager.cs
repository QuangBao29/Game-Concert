using System.Collections.Generic;
using EventData;
using UnityEngine;


public class CharacterManager : PersistentManager<CharacterManager>
{
    private GameObject _characterPrefab;
    private GameObject _character;

    private Animator _animator;
    private AnimationClip _dance;
    private RuntimeAnimatorController _characterAnimator;

    private string _currentCharacterPath;
    private readonly Vector3 _characterPosition = new(0.15f, 2.5f, 0f);
    private readonly Vector3 _characterRotation = new(0, 0, 0);

    private const string AnimationPath = "Assets/Animations/Dances/";
    private const string AnimatorPath = "Assets/Animations/Controllers/SMPL.controller";

    private void Start()
    {
        _characterAnimator = ResourceManager.LoadAnimator(AnimatorPath);
        _currentCharacterPath = "";
    }

    public void Dance(Component sender, object data)
    {
        _animator.Play("Base Layer." + _dance.name);
    }

    public void LoadCharacter(Component sender, object data)
    {
        if (_currentCharacterPath != PlayFabPlayerDataController.Instance.PlayerTitleData["Character Path"].Value)
        {
            _currentCharacterPath = PlayFabPlayerDataController.Instance.PlayerTitleData["Character Path"].Value;
            _characterPrefab = ResourceManager.LoadPrefabAsset(_currentCharacterPath);

            _character = Instantiate(_characterPrefab);
            _character.transform.position = _characterPosition;

            _animator = _character.GetComponent<Animator>();
            _animator.runtimeAnimatorController = _characterAnimator;
        }
    }

    public void ChangeCharacter(Component sender, object data)
    {
        Destroy(_character);
        ResourceManager.UnloadPrefabAsset(_characterPrefab);
        ResourceManager.UnloadAnimatorControllerAsset(_characterAnimator);

        var temp = (Dictionary<string, string>)data;
        _currentCharacterPath = temp["Character Path"];
        _characterPrefab = ResourceManager.LoadPrefabAsset(_currentCharacterPath);

        _character = Instantiate(_characterPrefab);
        _character.transform.position = _characterPosition;

        _animator = _character.GetComponent<Animator>();
        _animator.runtimeAnimatorController = _characterAnimator;
    }

    public void SetAnimation(Component sender, object data)
    {
        var temp = (LevelData)data;
        _dance = ResourceManager.LoadAnimationClip(AnimationPath + temp.SongName + ".anim");
    }


    public void Reset(Component sender, object data)
    {
        _character.transform.position = _characterPosition;
        _character.transform.eulerAngles = _characterRotation;
        _animator.Play("Idle");
    }
}