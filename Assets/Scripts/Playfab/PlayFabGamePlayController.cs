using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class PlayFabGamePlayController : PersistentManager<PlayFabGamePlayController>
{
    public GameEvent onPurchaseItemSuccess;

    public void StartPurchaseItem(Component sender, object data)
    {
        var tmp = (ItemInstance)data;
        var req = new PurchaseItemRequest
        {
            ItemId = tmp.ItemId,
            VirtualCurrency = tmp.UnitCurrency,
            Price = (int)tmp.UnitPrice
        };
        PlayFabClientAPI.PurchaseItem(req, OnPurchaseItemSuccess, PlayFabErrorHandler.Instance.HandleError);
    }

    private void OnPurchaseItemSuccess(PurchaseItemResult result)
    {
        onPurchaseItemSuccess.Invoke(this, null);
    }

    public void ConsumeItems(Component sender, object data)
    {
        var itemId = (string)data;
        var itemInstanceId = PlayFabPlayerDataController.Instance.GetItemInstanceId(itemId);

        if (string.IsNullOrEmpty(itemInstanceId))
        {
            return;
        }

        var playerData = new Dictionary<string, string>
        {
            { "Equip Item", "None" }
        };
        PlayFabPlayerDataController.Instance.SetPlayerData(this, playerData);

        var req = new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId
        };
        PlayFabClientAPI.ConsumeItem(req, _ =>
            {
                PlayFabPlayerDataController.Instance.GetAllData();
            },
            PlayFabErrorHandler.Instance.HandleError);
    }
}