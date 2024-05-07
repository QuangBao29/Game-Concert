using EventData;
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

    private ItemData _itemData;
    private Sprite _equipSlotSprite;

    protected override void Awake()
    {
        base.Awake();
        _itemData = Resources.Load<ItemData>("Scriptable Objects/Item Data");
    }

    public void OnPauseClick()
    {
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
        var equipItem = PlayFabPlayerDataController.Instance.PlayerTitleData["Equip Item"].Value;
        var equipSlotImage = equipSlot.transform.Find("Item").GetComponent<Image>();
        if (equipItem != "None")
        {
            if (!_equipSlotSprite)
            {
                _equipSlotSprite = ResourceManager.LoadSprite(_itemData.ItemPath + equipItem + ".asset");
            }

            equipSlotImage.sprite = _equipSlotSprite;
            equipSlotImage.color =
                new Color(equipSlotImage.color.r, equipSlotImage.color.g, equipSlotImage.color.b, 1.0f);
        }
        else
        {
            equipSlotImage.color =
                new Color(equipSlotImage.color.r, equipSlotImage.color.g, equipSlotImage.color.b, 0.0f);
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
}