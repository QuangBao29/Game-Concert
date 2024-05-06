using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UIShop : BaseUI
{
    public GameEvent onBuyClick;

    public GameObject contentDrawer;

    public AssetReferenceGameObject itemRef;


    private readonly Dictionary<string, Sprite> _listItemSprite = new();
    private GameObject _itemPrefab;

    private bool _isLoaded;
    private ItemData _itemData;

    protected override void Awake()
    {
        base.Awake();
        _isLoaded = false;
        _itemData = Resources.Load<ItemData>("Scriptable Objects/Item Data");
    }

    private void OnItemClick(string itemName, string description, string currency, int price)
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UIItemViewer, new UIItemViewerParam
        {
            ItemName = itemName,
            Description = description,
            Currency = currency,
            Price = price
        });
    }


    private void OnBuyClick(ItemInstance item)
    {
        PlaySoundOnClick();
        onBuyClick.Invoke(this, item);
    }

    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        if (!_isLoaded)
        {
            LoadShop();
            _isLoaded = true;
        }
    }

    private void LoadShop()
    {
        var catalogItems = PlayFabGameDataController.Instance.CatalogItems;

        _itemPrefab = ResourceManager.LoadPrefabAsset(itemRef);

        foreach (var item in catalogItems)
        {
            var shopItemGameObject = Instantiate(_itemPrefab, contentDrawer.transform, false);
            var buyButton = shopItemGameObject.transform.Find("Buy Button").GetComponent<Button>();
            var itemPrice = buyButton.transform.Find("Price").GetComponent<TextMeshProUGUI>();
            var currencyIcon = buyButton.transform.Find("Icon").GetComponent<Image>();
            
            var viewButton = shopItemGameObject.transform.Find("View Button");
            var itemIcon = shopItemGameObject.transform.Find("Item Icon");


            foreach (var (currency, price) in item.VirtualCurrencyPrices)
            {
                // Load Item
                Sprite itemSprite;
                if (!_listItemSprite.ContainsKey(item.DisplayName))
                {
                    itemSprite = ResourceManager.LoadSprite(_itemData.ItemPath + item.DisplayName + ".asset");
                    _listItemSprite.Add(item.DisplayName, itemSprite);
                }
                else
                {
                    itemSprite = _listItemSprite[item.DisplayName];
                }

                // Load Icon
                Sprite iconSprite;
                if (!_listItemSprite.ContainsKey(currency))
                {
                    iconSprite = ResourceManager.LoadSprite(_itemData.ItemPath + currency + ".png");
                    _listItemSprite.Add(currency, iconSprite);
                }
                else
                {
                    iconSprite = _listItemSprite[currency];
                }

                viewButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnItemClick(item.DisplayName, item.Description, currency, (int)price);
                });

                buyButton.onClick.AddListener(() =>
                {
                    OnBuyClick(new ItemInstance
                    {
                        ItemId = item.ItemId,
                        UnitCurrency = currency,
                        UnitPrice = price
                    });
                });

                itemPrice.SetText(price.ToString());
                currencyIcon.sprite = iconSprite;
                itemIcon.GetComponent<Image>().sprite = itemSprite;

            }
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        foreach (var (_, sprite) in _listItemSprite)
        {
            ResourceManager.UnloadSpriteAsset(sprite);
        }

        _listItemSprite.Clear();
    }

    public void OnCharacterCategoryClick()
    {
    }

    public void OnItemCategoryClick()
    {
    }
}