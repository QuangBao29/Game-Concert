using System;
using EventData;
using UnityEngine;

public class AudioManager : PersistentManager<AudioManager>
{
    public AudioSource musicChannel;
    public AudioSource soundFxChannel;

    private SongData _songData;
    private AudioData _audioData;

    public GameEvent onSongEnd;

    private bool _isSongEndInvoke;
    private int _songIndex;

    public override void Awake()
    {
        base.Awake();
        _songData = Resources.Load<SongData>("Scriptable Objects/Song Data");
        _audioData = Resources.Load<AudioData>("Scriptable Objects/Audio Data");
        _songIndex = -1;
        _isSongEndInvoke = false;
    }

    private void Start()
    {
        musicChannel.volume = PlayerPrefs.GetFloat("Music Volume");
        soundFxChannel.volume = PlayerPrefs.GetFloat("Sound Volume");
    }

    public void UpdateVolume(Component sender, object data)
    {
        musicChannel.volume = PlayerPrefs.GetFloat("Music Volume");
        soundFxChannel.volume = PlayerPrefs.GetFloat("Sound Volume");
    }

    public void SetSong(Component sender, object data)
    {
        var temp = (LevelData)data;
        _songIndex = temp.SongIndex;
    }


    public void PlaySong(Component sender, object data)
    {
        if (_songIndex != -1)
        {
            _isSongEndInvoke = false;
            var audioClip = ResourceManager.LoadAudioClip(_songData.SongPath + _songData.ListSong[_songIndex].Title);
            musicChannel.clip = audioClip;
            musicChannel.Play();
        }
    }

    public void PauseSong(Component sender, object data)
    {
        if (!musicChannel) return;
        musicChannel.Pause();
    }

    public void StopSong(Component sender, object data)
    {
        if (!musicChannel) return;
        musicChannel.Stop();
    }

    public void UnPauseSong(Component sender, object data)
    {
        if (!musicChannel) return;
        musicChannel.UnPause();
    }

    private void RemoveSong()
    {
        if (!musicChannel) return;
        ResourceManager.UnloadAudioClipAsset(musicChannel.clip);
        musicChannel.clip = null;
    }

    public void PlaySoundFx(Component sender, object data)
    {
        if (data is int audioIndex)
        {
            var audioClip = ResourceManager.LoadAudioClip(_audioData.AudioPath + _audioData.ListAudio[audioIndex]);
            soundFxChannel.clip = audioClip;
            soundFxChannel.Play();
        }
    }

    public void PauseSoundFx()
    {
        if (!soundFxChannel) return;
        soundFxChannel.Pause();
    }

    public void RemoveSoundFx()
    {
        if (!soundFxChannel) return;
        soundFxChannel.clip = null;
    }


    public void Reset(Component sender, object data)
    {
        RemoveSong();
        _songIndex = -1;
        _isSongEndInvoke = false;
    }

    private void Update()
    {
        if (!musicChannel.isPlaying && !_isSongEndInvoke && musicChannel.clip != null && Application.isFocused)
        {
            onSongEnd.Invoke(this, null);
            _isSongEndInvoke = true;
        }
    }
}