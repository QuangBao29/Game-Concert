using System.IO;
using TMPro;
using UI;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.Networking;
using GameData;
using PlayFab.ClientModels;

public class UILevelEditor : BaseUI
{
    public TMP_InputField pathTxt;
    public TextMeshProUGUI InfoText;
    private CustomLevelData _customLevelData;

    protected override void Awake()
    {
        base.Awake();
        _customLevelData = Resources.Load<CustomLevelData>("Scriptable Objects/Custom Level Data");
    }

    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        pathTxt.text = _customLevelData.FolderPath;
        if (_customLevelData.FolderPath != "")
        {
            DisplayInfo();
        }
    }

    private void DisplayInfo()
    {
        var info = "";
        for (var i = 0; i < _customLevelData.ListSong.Count; i++)
        {
            info += $"song {i + 1}: {_customLevelData.ListSong[i].SongName} imported successfully! \n";
        }
        InfoText.SetText(info);
    }

    private void ResetCustomLevelData()
    {
        _customLevelData.FolderPath = "";
        _customLevelData.BundlePath = "";
        _customLevelData.MIDIPath = "";
        _customLevelData.SongPath = "";
        _customLevelData.ListSong.Clear();
    }

    public void OnSetPathClick()
    {
        PlaySoundOnClick();
        ResetCustomLevelData();
        if (Directory.Exists(pathTxt.text))
        {
            _customLevelData.FolderPath = pathTxt.text;
            _customLevelData.BundlePath = pathTxt.text + "/Anim";
            _customLevelData.MIDIPath = pathTxt.text + "/MIDI";
            _customLevelData.SongPath = pathTxt.text + "/Music";

            DirectoryInfo dir = new DirectoryInfo(_customLevelData.SongPath);
            var songDir = dir.GetFiles("*.wav").Concat(dir.GetFiles("*.mp3")).ToList();
            var count = songDir.Count();

            dir = new DirectoryInfo(_customLevelData.BundlePath);
            var animDir = dir.GetFiles("*.anim").ToList();

            dir = new DirectoryInfo(_customLevelData.MIDIPath);
            var MIDIDir = dir.GetFiles("*.mid").Concat(dir.GetFiles("*.midi")).ToList();
            for (var i = 0; i < count; i++)
            {
                _customLevelData.ListSong.Add(new CustomSongData
                {
                    SongName = songDir[i].Name,
                    SongPath = songDir[i].ToString(),
                    MIDIPath = MIDIDir[i].ToString(),
                    AnimationPath = animDir[i].Name,
                });
            }
        }
        DisplayInfo();
    }


    public void OnBackClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UINavigationTab);
        UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
    }
}
