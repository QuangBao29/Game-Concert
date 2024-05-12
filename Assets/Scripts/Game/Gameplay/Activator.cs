using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Activator : MonoBehaviour
{
    public KeyCode KeyInput;
    public GameEvent OnNoteHit;
    [SerializeField]
    private NoteManager noteManager = null;
    [SerializeField]
    private GameObject endZone = null;
    [SerializeField]
    private GameObject startZone = null;
    [SerializeField]
    private GameObject hitZone = null;

    private List<Double> noteTimeMidi = new(); //timestamp that note spawned (based on midi)
    private List<Note> notes = new();
    private List<NoteName> pitches = new();
    private int spawnIndex = 0;
    private int noteIndex = 0;
    private int zoneIndex = 0;
    private float cooldown = Define.HitObjectInterval;
    private float lastClickedTime = 0;

    /// <summary>
    /// from 0 to 1
    /// </summary>
    public int ZoneIndex
    {
        get => zoneIndex;
        set => zoneIndex = value;
    }
    public List<NoteName> Pitches
    {
        get => pitches;
        set => pitches = value;
    }
    public List<Double> NoteTimeMidi
    {
        get => noteTimeMidi;
        set => noteTimeMidi = value;
    }
    public GameObject StartZone
    {
        get => startZone;
    }
    public GameObject EndZone
    {
        get => endZone;
    }
    private void Start()
    {
        //originalHitPos = hitZone.transform.position;
    }
    void Update()
    {
        if (spawnIndex < noteTimeMidi.Count)
        {
            //spawn note truoc 1 khoang thoi gian NoteTime
            if (SongManager.GetAudioSourceTime() >= noteTimeMidi[spawnIndex] - SongManager.Instance.NoteTime)
            {
                var note = noteManager.OnSpawnNotesToTarget(startZone.transform.position, endZone.transform.position, hitZone.transform.position);
                notes.Add(note);
                spawnIndex++;
            }
        }

        if (Input.GetKeyDown(KeyInput) && Time.time - lastClickedTime > cooldown)
        {
            //check notes[0] xem thoi gian khi button dc click co nam trong khoang thoi gian cho phep hay khong
            if (notes.Count != 0)
            {
                //add effect for button
                lastClickedTime = Time.time;
                var note = notes[0];
                double timeStamp = noteTimeMidi[noteIndex];
                double marginOfError = SongManager.Instance.MarginOfError;
                double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.InputDelayInMilliseconds / 1000.0);
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    //Debug.LogError("Hit on note");
                    OnNoteHit.Invoke(null, HitType.Perfect);
                }
                else
                {
                    //Debug.LogError(String.Format("Hit inaccurate on note with {0} delay", audioTime - timeStamp));
                    OnNoteHit.Invoke(null, HitType.Miss);
                }
                notes.RemoveAt(0);
                note.OnFinishNote();
                noteIndex++;
            }
        }
    }

    public void OnResponseNoteMiss(Component component, object data)
    {
        if (notes.Count == 0)
        {
            return;
        }
        var note = notes[0];
        notes.RemoveAt(0);
        note.OnFinishNote();
        noteIndex++;
    }
}
