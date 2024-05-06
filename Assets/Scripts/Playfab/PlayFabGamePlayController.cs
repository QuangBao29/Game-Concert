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
        PlayFabClientAPI.PurchaseItem(req, OnPurchaseItemSuccess, PlayFabErrorHandler.HandleError);
    }

    private void OnPurchaseItemSuccess(PurchaseItemResult result)
    {
        onPurchaseItemSuccess.Invoke(this, null);
    }

    public void ConsumeItems(Component sender, object data)
    {
        var req = new ConsumeItemRequest
        {
            ConsumeCount = 1,
            // ItemInstanceId = id
        };
        PlayFabClientAPI.ConsumeItem(req, null, PlayFabErrorHandler.HandleError);
    }
}