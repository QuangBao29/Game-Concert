using System.Collections.Generic;
using EventData;
using PlayFab.ClientModels;
using UnityEngine;
using PlayFab;

public class PlayFabPlayerDataController : PersistentManager<PlayFabPlayerDataController>
{
    public string PlayerId { get; set; }
    private Dictionary<string, int> Currencies { get; } = new();
    public List<ItemInstance> Inventory { get; } = new();
    public Dictionary<string, UserDataRecord> PlayerTitleData;
    public UserData PlayerData;
    public GameEvent onPlayerTitleDataRetrieved;
    public GameEvent onInventoryUpdated;

    public void GetAllData()
    {
        GetAccountInfo();
        GetPlayerData();
    }

    private void GetAccountInfo()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            PlayFabId = PlayerId
        }, result =>
        {
            PlayerData = new UserData
            {
                Username = result.AccountInfo.Username
            };
            GetInventory(null, null);
        }, PlayFabErrorHandler.Instance.HandleError);
    }

    public void GetInventory(Component sender, object data)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            Currencies.Clear();
            foreach (var pair in result.VirtualCurrency)
            {
                Currencies.Add(pair.Key, pair.Value);
            }

            PlayFabFlags.Instance.Currency = true;

            Inventory.Clear();
            foreach (var item in result.Inventory)
            {
                Inventory.Add(item);
            }

            PlayFabFlags.Instance.Inventory = true;

            PlayerData.Coin = Currencies["CN"];
            PlayerData.Gem = Currencies["GM"];
            onInventoryUpdated.Invoke(this, PlayerData);
        }, PlayFabErrorHandler.Instance.HandleError);
    }

    public void SetPlayerData(Component sender, object data)
    {
        var tmp = (Dictionary<string, string>)data;

        var req = new UpdateUserDataRequest
        {
            Data = tmp
        };

        PlayFabClientAPI.UpdateUserData(req, _ =>
        {
            if (PlayerTitleData != null)
            {
                foreach (var key in tmp.Keys)
                {
                    UserDataRecord value = new() { Value = tmp[key] };
                    PlayerTitleData[key] = value;
                }
            }
        }, PlayFabErrorHandler.Instance.HandleError);
    }


    private void GetPlayerData()
    {
        var listDataKeys = Resources.Load<PlayerData>("Scriptable Objects/Player Data Key").PlayerDataKeys;

        var req = new GetUserDataRequest
        {
            Keys = listDataKeys,
            PlayFabId = PlayerId
        };
        PlayFabClientAPI.GetUserData(req, result =>
            {
                PlayerTitleData?.Clear();
                PlayerTitleData = result.Data;
                
                onPlayerTitleDataRetrieved.Invoke(this, null);
                PlayFabFlags.Instance.TitleData = true;
            },
            PlayFabErrorHandler.Instance.HandleError);
    }

    public void AddCurrency(Component sender, object data)
    {
        var temp = (RewardData)data;
        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = temp.CoinKey,
            Amount = temp.CoinAmount
        }, _ =>
        {
            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
            {
                VirtualCurrency = temp.GemKey,
                Amount = temp.GemAmount
            }, _ => { GetInventory(this, null); }, PlayFabErrorHandler.Instance.HandleError);
        }, PlayFabErrorHandler.Instance.HandleError);
    }

    public string GetItemInstanceId(string displayName)
    {
        if (displayName == "None")
        {
            return string.Empty;
        }

        foreach (var item in Inventory)
        {
            if (item.DisplayName == displayName)
            {
                return item.ItemInstanceId;
            }
        }

        return string.Empty;
    }
}