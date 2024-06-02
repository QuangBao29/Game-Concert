using System.Collections.Generic;
using EventData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class CharacterManager : PersistentManager<CharacterManager>
{
    private GameObject _characterPrefab;
    private GameObject _character;
    private List<GameObject> _listNPCs;
    private List<GameObject> _listNPCsPrefab;
    public List<Vector3> ListPosition;
    private Animator _animator;
    private AnimationClip _dance;
    private RuntimeAnimatorController _characterAnimatorController;

    private string _currentCharacterPath;
    private readonly Vector3 _characterPosition = new(0.15f, 2.5f, 0f);
    private readonly Vector3 _characterRotation = new(0, 0, 0);

    private const string AnimationPath = "Assets/Animations/Dances/";
    public AssetReference animatorRef;

    public override async void Awake()
    {
        base.Awake();
        var handleCharacterAnimator = Addressables.LoadAssetAsync<RuntimeAnimatorController>(animatorRef);
        await handleCharacterAnimator.Task;
        if (handleCharacterAnimator.Status == AsyncOperationStatus.Succeeded)
        {
            _characterAnimatorController = handleCharacterAnimator.Result;
        }

        _listNPCs = new List<GameObject>();
        _listNPCsPrefab = new List<GameObject>();

        Application.quitting += () => { Addressables.Release(handleCharacterAnimator); };
    }

    private void Start()
    {
        _currentCharacterPath = "";
    }

    public void Dance(Component sender, object data)
    {
        foreach (var character in _listNPCs)
        {
            character.GetComponent<Animator>().Play(_dance.name);
        }
        _animator.Play(_dance.name);
    }

    public void LoadCharacter(Component sender, object data)
    {
        if (_currentCharacterPath != PlayFabPlayerDataController.Instance.PlayerTitleData["Character Path"].Value)
        {
            if (_character)
            {
                Destroy(_character);
                ResourceManager.UnloadPrefabAsset(_characterPrefab);
            }

            _currentCharacterPath = PlayFabPlayerDataController.Instance.PlayerTitleData["Character Path"].Value;
            _characterPrefab = ResourceManager.LoadPrefabAsset(_currentCharacterPath);

            _character = Instantiate(_characterPrefab);
            _character.transform.position = _characterPosition;

            _animator = _character.GetComponent<Animator>();
            _animator.runtimeAnimatorController = _characterAnimatorController;
        }
    }

    public void ChangeCharacter(Component sender, object data)
    {
        Destroy(_character);
        ResourceManager.UnloadPrefabAsset(_characterPrefab);

        var temp = (Dictionary<string, string>)data;
        _currentCharacterPath = temp["Character Path"];
        _characterPrefab = ResourceManager.LoadPrefabAsset(_currentCharacterPath);

        _character = Instantiate(_characterPrefab);
        _character.transform.position = _characterPosition;

        _animator = _character.GetComponent<Animator>();
        _animator.runtimeAnimatorController = _characterAnimatorController;
    }

    public void SetAnimation(Component sender, object data)
    {
        var temp = (LevelData)data;
        _dance = ResourceManager.LoadAnimationClip(AnimationPath + temp.SongName + ".anim");
    }

    public async void SetCustomAnimation(Component sender, object data)
    {
        var temp = (UserLevelData)data;
        _dance = await ResourceManager.Instance.LoadLocalAnimationClip(temp.BundlePath, temp.AnimClip);
        var overrideController = new AnimatorOverrideController(_characterAnimatorController);
        overrideController["Idle 1"] = _dance;
        _dance.name = "Idle 1";
        _animator.runtimeAnimatorController = overrideController;
        foreach (var character in _listNPCs)
        {
            character.GetComponent<Animator>().runtimeAnimatorController = overrideController;
        }
    }

    public void UnloadNPC(Component sender, object data)
    {
        foreach (var npc in _listNPCsPrefab)
        {
            ResourceManager.UnloadPrefabAsset(npc);
        }
        foreach (var npc in _listNPCs)
        {
            Destroy(npc);
        }
    }

    public void LoadNPC(Component sender, object data)
    {
        var tmp = Resources.Load<SettingData>("Scriptable Objects/Setting Data");
        var listNPC = tmp.NPC;
        for (var i = 0; i < listNPC.Count; i++)
        {
            var npcPrefab = ResourceManager.LoadPrefabAsset(listNPC[i]);
            _listNPCsPrefab.Add(npcPrefab);

            var NPC = Instantiate(npcPrefab);
            NPC.transform.position = ListPosition[i];

            var animator = NPC.GetComponent<Animator>();
            animator.runtimeAnimatorController = _characterAnimatorController;
            _listNPCs.Add(NPC);
        }
    }

    public void Reset(Component sender, object data)
    {
        for (var i = 0; i < _listNPCs.Count; i++)
        {
            _listNPCs[i].transform.position = ListPosition[i];
            _listNPCs[i].transform.eulerAngles = _characterRotation;
            _listNPCs[i].GetComponent<Animator>().Play("Idle");
        }
        _character.transform.position = _characterPosition;
        _character.transform.eulerAngles = _characterRotation;
        _animator.Play("Idle");
        _animator.runtimeAnimatorController = _characterAnimatorController;
    }
}