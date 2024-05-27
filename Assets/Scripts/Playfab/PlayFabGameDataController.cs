using System;
using System.Collections.Generic;
using EventData;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class PlayFabGameDataController : PersistentManager<PlayFabGameDataController>
{
    private readonly List<CatalogItem> _catalogItems = new();
    public IEnumerable<CatalogItem> CatalogItems => _catalogItems;
    public List<PlayerLeaderboardEntry> CurrentLeaderBoard { get; private set; } = new();
    public int PlayerRank { get; private set; }


    private void GetCatalogItems(string catalogVersion = "1")
    {
        var req = new GetCatalogItemsRequest
        {
            CatalogVersion = catalogVersion
        };

        PlayFabClientAPI.GetCatalogItems(req, result =>
        {
            foreach (var item in result.Catalog)
            {
                _catalogItems.Add(item);
            }

            PlayFabFlags.Instance.Catalog = true;
        }, PlayFabErrorHandler.Instance.HandleError);
    }

    public void GetLeaderBoard(Component sender, object data)
    {
        if (data is LeaderBoardReqInfo tmp)
        {
            var req = new GetLeaderboardRequest
            {
                MaxResultsCount = 10,
                StartPosition = 0,
                StatisticName = tmp.Name
            };
            PlayFabClientAPI.GetLeaderboard(req, result =>
                {
                    CurrentLeaderBoard = result.Leaderboard;
                    GetPlayerRank(tmp.Name, () => { tmp.SuccessCallback?.Invoke(); }
                    );
                },
                PlayFabErrorHandler.Instance.HandleError
            );
        }
    }

    private void GetPlayerRank(string statName, Action callback)
    {
        var req = new GetLeaderboardAroundPlayerRequest
        {
            MaxResultsCount = 1,
            StatisticName = statName
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(req,
            result =>
            {
                if (CurrentLeaderBoard.Count == result.Leaderboard[0].Position)
                {
                    PlayerRank = -1;
                }
                else
                {
                    PlayerRank = result.Leaderboard[0].Position + 1;
                }
                callback?.Invoke();
            },
            PlayFabErrorHandler.Instance.HandleError
        );
    }

    public void SendLeaderBoard(Component sender, object data)
    {
        var temp = (UpdateLeaderBoardReqInfo)data;
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new()
                {
                    StatisticName = temp.Name,
                    Value = temp.Score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(req, _ => {
            GetLeaderBoard(this, new LeaderBoardReqInfo
            {
                Name = temp.Name,
                SuccessCallback = temp.SuccessCallback
            });
        },
            PlayFabErrorHandler.Instance.HandleError);
    }

    public void GetAllData()
    {
        GetCatalogItems();
    }
}