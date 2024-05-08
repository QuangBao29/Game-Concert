using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UIInventory : BaseUI
{
    public GameEvent onEquipClick;
    public GameEvent onUnEquipClick;

    public GameObject equipSlot;
    public GameObject contentDrawer;

    public AssetReference itemSlotRef;

    private GameObject _itemSlotPrefab;
    private readonly Dictionary<string, Sprite> _listItemSprite = new();
    private string _equipItem;

    private List<ItemInstance> _inventory;
    private ItemData _itemData;

    protected override void Awake()
    {
        base.Awake();
        _itemData = Resources.Load<ItemData>("Scriptable Objects/Item Data");
    }

    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        LoadInventory();
    }

    protected override void OnHide()
    {
        base.OnHide();
        DestroyAllContent();
    }

    private void DestroyAllContent()
    {
        foreach (var sprite in _listItemSprite)
        {
            ResourceManager.UnloadSpriteAsset(sprite.Value);
        }

        ResourceManager.UnloadPrefabAsset(_itemSlotPrefab);
        _listItemSprite.Clear();

        foreach (Transform child in contentDrawer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void DestroyInventoryItems()
    {
        foreach (Transform child in contentDrawer.transform)
        {
            Destroy(child.gameObject);
        }
    }


    private void LoadEquipSlot(string itemName, Sprite itemSprite)
    {
        _equipItem = itemName;
        var imageComponent = equipSlot.transform.Find("Sprite").GetComponent<Image>();
        imageComponent.sprite = itemSprite;
        imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 1f);
    }

    private void RemoveEquipSlot()
    {
        _equipItem = "None";
        var imageComponent = equipSlot.transform.Find("Sprite").GetComponent<Image>();
        imageComponent.sprite = null;
        imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 0f);
    }

    private void OnEquipClick(string itemName)
    {
        PlaySoundOnClick();
        _equipItem = itemName;
        DestroyInventoryItems();
        ReloadInventory();
        onEquipClick.Invoke(this, new Dictionary<string, string>
        {
            { "Equip Item", _equipItem }
        });
    }

    private void ReloadInventory()
    {
        foreach (var item in _inventory)
        {
            if (item.RemainingUses == 1 && item.DisplayName == _equipItem)
            {
                LoadEquipSlot(item.DisplayName, _listItemSprite[item.DisplayName]);
                continue;
            }

            var invItemGameObject = Instantiate(_itemSlotPrefab, contentDrawer.transform, false);
            var invItemImage = invItemGameObject.transform.Find("Sprite").GetComponent<Image>();
            var invItemAmount = invItemGameObject.transform.Find("Amount").GetComponent<TextMeshProUGUI>();

            invItemImage.sprite = _listItemSprite[item.DisplayName];

            invItemGameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnEquipClick(item.DisplayName);
                invItemAmount.SetText((item.RemainingUses - 1 ?? 0).ToString());
            });


            if (item.DisplayName == _equipItem)
            {
                LoadEquipSlot(item.DisplayName, _listItemSprite[item.DisplayName]);
                invItemAmount.SetText((item.RemainingUses - 1 ?? 0).ToString());
            }
            else
            {
                invItemAmount.SetText((item.RemainingUses ?? 0).ToString());
            }
        }
    }

    private void LoadInventory()
    {
        _equipItem = PlayFabPlayerDataController.Instance.PlayerTitleData["Equip Item"].Value;
        _inventory = PlayFabPlayerDataController.Instance.Inventory;

        _itemSlotPrefab = ResourceManager.LoadPrefabAsset(itemSlotRef);

        foreach (var item in _inventory)
        {
            if (item.RemainingUses == 1 && item.DisplayName == _equipItem)
            {
                var equipItemSprite = ResourceManager.LoadSprite(_itemData.ItemPath + item.DisplayName + ".asset");
                _listItemSprite.Add(item.DisplayName, equipItemSprite);
                LoadEquipSlot(item.DisplayName, equipItemSprite);
                continue;
            }

            var invItemGameObject = Instantiate(_itemSlotPrefab, contentDrawer.transform, false);
            var invItemImage = invItemGameObject.transform.Find("Sprite").GetComponent<Image>();
            var invItemAmount = invItemGameObject.transform.Find("Amount").GetComponent<TextMeshProUGUI>();

            var itemSprite = ResourceManager.LoadSprite(_itemData.ItemPath + item.DisplayName + ".asset");

            _listItemSprite.Add(item.DisplayName, itemSprite);
            invItemImage.sprite = itemSprite;

            invItemGameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnEquipClick(item.DisplayName);
                invItemAmount.SetText((item.RemainingUses - 1 ?? 0).ToString());
            });


            if (item.DisplayName == _equipItem)
            {
                LoadEquipSlot(item.DisplayName, itemSprite);
                invItemAmount.SetText((item.RemainingUses - 1 ?? 0).ToString());
            }
            else
            {
                invItemAmount.SetText((item.RemainingUses ?? 0).ToString());
            }
        }
    }

    public void OnUnEquipClick()
    {
        PlaySoundOnClick();
        RemoveEquipSlot();
        DestroyInventoryItems();
        ReloadInventory();
        onUnEquipClick.Invoke(this, new Dictionary<string, string>
        {
            { "Equip Item", _equipItem }
        });
    }
}