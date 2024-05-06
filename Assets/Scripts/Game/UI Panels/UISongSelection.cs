using EventData;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UISongSelection : BaseUI
{
    public GameEvent onPlayClick;
    public GameEvent onSongClick;

    public AssetReference songRef;
    public AssetReference playerRef;
    public GameObject listSongContentDrawer;
    public GameObject listPlayerContentDrawer;


    public TextMeshProUGUI playerRankTxt;
    public TextMeshProUGUI warningTxt;
    public GameObject leaderBoardWarningText;

    private SongData _songData;
    private int _songIndex;

    protected override void Awake()
    {
        base.Awake();
        _songData = Resources.Load<SongData>("Scriptable Objects/Song Data");
        _songIndex = -1;
    }

    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        LoadSongList();
    }


    public void OnPlayClick()
    {
        if (_songIndex != -1)
        {
            onPlayClick.Invoke(this, new LevelData
            {
                SongName = _songData.ListSong[_songIndex].Title,
                SongIndex = _songIndex
            });
        }
    }

    private void OnSongClick(int songIndex)
    {
        _songIndex = songIndex;
        PlaySoundOnClick();
        onSongClick.Invoke(this, new LeaderBoardReqInfo
        {
            Name = _songData.ListSong[_songIndex].Title,
            SuccessCallback = LoadLeaderBoard
        });
    }

    private void LoadSongList()
    {
        var songPrefab = ResourceManager.LoadPrefabAsset(songRef);
        if (songPrefab)
        {
            for (var i = 0; i < _songData.ListSong.Count; i++)
            {
                var songGameObject = Instantiate(songPrefab, listSongContentDrawer.transform, false);

                var songTitle = songGameObject.transform.Find("Song Title").GetComponent<TextMeshProUGUI>();
                var songArtist = songGameObject.transform.Find("Artist Name").GetComponent<TextMeshProUGUI>();
                var songCover = songGameObject.transform.Find("Cover").GetComponent<Image>();

                songTitle.SetText(_songData.ListSong[i].Title);
                songArtist.SetText(_songData.ListSong[i].Artist);

                var songIndex = i;
                songGameObject.GetComponent<Button>().onClick.AddListener(() => OnSongClick(songIndex));
            }
        }
    }

    private void LoadLeaderBoard()
    {
        foreach (Transform child in listPlayerContentDrawer.transform)
        {
            Destroy(child.gameObject);
        }

        var playerPrefab = ResourceManager.LoadPrefabAsset(playerRef);
        if (playerPrefab)
        {
            var leaderBoard = PlayFabGameDataController.Instance.CurrentLeaderBoard;
            if (leaderBoard.Count != 0)
            {
                leaderBoardWarningText.SetActive(false);
                foreach (var player in leaderBoard)
                {
                    var playerGameObject = Instantiate(playerPrefab, listPlayerContentDrawer.transform, false);
                    playerGameObject.transform.Find("Avatar").GetComponent<Image>();
                    var playerName = playerGameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                    var playerScore = playerGameObject.transform.Find("Score").GetComponent<TextMeshProUGUI>();

                    playerName.SetText(player.DisplayName);
                    playerScore.SetText(player.StatValue.ToString());
                }

                ShowPlayerRank();
            }
            else
            {
                ShowWarning();
            }
        }
    }

    private void ShowWarning()
    {
        leaderBoardWarningText.SetActive(true);
        warningTxt.SetText("No Record");
    }

    private void ShowPlayerRank()
    {
        playerRankTxt.SetText("Rank: #" + PlayFabGameDataController.Instance.PlayerRank);
    }
}