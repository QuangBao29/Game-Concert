using PlayFab.ClientModels;
using TMPro;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIItemViewer : BaseUI
{
    public GameEvent onBuyClick;

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI price;
    public Image item;
    public Image currencyIcon;

    private ItemData _itemData;
    private Sprite _itemSprite;
    private Sprite _iconSprite;

    private string _itemId;
    private string _currency;
    private uint _price;

    protected override void Awake()
    {
        base.Awake();
        _itemData = Resources.Load<ItemData>("Scriptable Objects/Item Data");
    }

    public void OnBuyClick()
    {
        PlaySoundOnClick();
        onBuyClick.Invoke(this, new ItemInstance
        {
            ItemId = _itemId,
            UnitCurrency = _currency,
            UnitPrice = _price
        });
    }

    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        var tmp = (UIItemViewerParam)param;
        if (tmp != null)
        {
            _price = tmp.Price;
            _itemId = tmp.ItemId;
            _currency = tmp.Currency;

            price.SetText(tmp.Price.ToString());
            descriptionText.SetText(tmp.Description);
            _iconSprite = ResourceManager.LoadSprite(_itemData.ItemPath + tmp.Currency + ".png");
            _itemSprite = ResourceManager.LoadSprite(_itemData.ItemPath + tmp.ItemName + ".asset");

            currencyIcon.sprite = _iconSprite;
            item.sprite = _itemSprite;
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        ResourceManager.UnloadSpriteAsset(_iconSprite);
        ResourceManager.UnloadSpriteAsset(_itemSprite);
    }
}