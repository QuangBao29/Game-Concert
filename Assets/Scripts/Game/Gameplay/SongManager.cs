using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.Networking;
using System;

public class SongManager : SingletonMono<SongManager>
{
    public static MidiFile Midifile = null;
    [SerializeField] private ActivatorManager _ActivatorManager = null;
    [SerializeField] private float _songDelayInSeconds = 0;
    [SerializeField] private double _marginOfError = 0;
    [SerializeField] private float _noteTime = 1;

    [SerializeField] private int _inputDelayInMilliseconds = 0;

    private string _songName = "TakeMeToYourHeart";

    public float NoteTime
    {
        get => _noteTime;
        set => _noteTime = value;
    }

    public double MarginOfError
    {
        get => _marginOfError;
        set => _marginOfError = value;
    }

    public float SongDelayInSeconds
    {
        get => _songDelayInSeconds;
        set => _songDelayInSeconds = value;
    }

    public int InputDelayInMilliseconds
    {
        get => _inputDelayInMilliseconds;
        set => _inputDelayInMilliseconds = value;
    }

    public void ReadFromFile(string songName)
    {
        string midiFileName = songName + Define.MidiFileExtension;
        string midiPath = Path.Combine(Define.MidiFilePath, midiFileName);
        Midifile = MidiFile.Read(Application.streamingAssetsPath + midiPath);
        GetDataFromMidi();
    }

    private void GetDataFromMidi()
    {
        var notes = Midifile.GetNotes();
        List<Melanchall.DryWetMidi.Interaction.Note> listNote = new();
        listNote.AddRange(notes);
        _ActivatorManager.SetSpawnedTimes(listNote);
    }

    /// <summary>
    /// return current real-time in audio clip played.
    /// </summary>
    public static double GetAudioSourceTime()
    {
        return (double)AudioManager.Instance.musicChannel.timeSamples /
               AudioManager.Instance.musicChannel.clip.frequency;
    }
}