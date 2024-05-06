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
        }, PlayFabErrorHandler.HandleError);
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
            
            GetPlayerRank(tmp.Name, () =>
            {
                PlayFabClientAPI.GetLeaderboard(req, result =>
                    {
                        CurrentLeaderBoard = result.Leaderboard;
                        tmp.SuccessCallback();
                    },
                    PlayFabErrorHandler.HandleError
                );
            });

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
                PlayerRank = result.Leaderboard[0].Position + 1;
                callback();
            },
            PlayFabErrorHandler.HandleError
        );
    }

    public static void SendLeaderBoard(string leaderBoardName, int score)
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new()
                {
                    StatisticName = leaderBoardName,
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(req, null, PlayFabErrorHandler.HandleError);
    }

    public void GetAllData()
    {
        GetCatalogItems();
    }
}