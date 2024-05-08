using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIHud : BaseUI
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI multiplier;
    public TextMeshProUGUI streak;

    public GameObject equipSlot;
    public GameEvent onPauseClick;
    public GameEvent onUseItem;

    private ItemData _itemData;
    private Sprite _equipSlotSprite;
    private string _equipItem;
    private Image _equipSlotImage;

    protected override void Awake()
    {
        base.Awake();
        _itemData = Resources.Load<ItemData>("Scriptable Objects/Item Data");
        _equipItem = "None";
    }

    public void OnPauseClick()
    {
        PlaySoundOnClick();
        onPauseClick.Invoke(this, null);
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UIPause);
    }

    public void UpdateScore(Component sender, object data)
    {
        if (data is int amount)
        {
            SetScore(amount.ToString());
        }
    }

    public void UpdateStreak(Component sender, object data)
    {
        if (data is int streak)
        {
            SetStreak(streak.ToString());
        }
    }

    public void UpdateMultiplier(Component sender, object data)
    {
        if (data is int combo)
        {
            SetMultiplier(combo.ToString());
        }
    }

    private void SetScore(string newScore)
    {
        score.SetText(newScore);
    }

    private void SetStreak(string newStreak)
    {
        streak.SetText(newStreak);
    }

    private void SetMultiplier(string newMultiplier)
    {
        multiplier.SetText(newMultiplier);
    }

    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        _equipItem = PlayFabPlayerDataController.Instance.PlayerTitleData["Equip Item"].Value;
        _equipSlotImage = equipSlot.transform.Find("Item").GetComponent<Image>();
        if (_equipItem != "None")
        {
            if (!_equipSlotSprite)
            {
                _equipSlotSprite = ResourceManager.LoadSprite(_itemData.ItemPath + _equipItem + ".asset");
            }

            _equipSlotImage.sprite = _equipSlotSprite;
            _equipSlotImage.color =
                new Color(_equipSlotImage.color.r, _equipSlotImage.color.g, _equipSlotImage.color.b, 1.0f);
        }
        else
        {
            _equipSlotImage.color =
                new Color(_equipSlotImage.color.r, _equipSlotImage.color.g, _equipSlotImage.color.b, 0.0f);
        }
    }

    public void EndGame(Component sender, object data)
    {
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UIVictory, new UIParam
        {
            Data = data
        });
    }

    public void OnUseItemClick()
    {
        PlaySoundOnClick();
        if (_equipItem == "None") return;
        _equipSlotImage.color =
            new Color(_equipSlotImage.color.r, _equipSlotImage.color.g, _equipSlotImage.color.b, 0.0f);
        onUseItem.Invoke(this, _equipItem);
    }

}