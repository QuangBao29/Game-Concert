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
    private int inputIndex = 0;
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
            OnClickHitButton();
            //check notes[0] xem thoi gian khi button dc click co nam trong khoang thoi gian cho phep hay khong
            var note = notes[0];
            double timeStamp = noteTimeMidi[0];
            double marginOfError = SongManager.Instance.MarginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.InputDelayInMilliseconds / 1000.0);
            if (Math.Abs(audioTime - timeStamp) < marginOfError)
            {
                Debug.LogError("Hit on note");
                OnNoteHit.Invoke(null, HitType.Perfect);
            }
            else
            {
                Debug.LogError(String.Format("Hit inaccurate on note with {0} delay", Math.Abs(audioTime - timeStamp)));
                OnNoteHit.Invoke(null, HitType.Miss);
            }
            var temp = notes[0];
            notes.RemoveAt(0);
            temp.OnFinishNote();
        }
        //if (hitZone.transform.position != originalHitPos && isClicked)
        //{
        //    Invoke("OnClickUpKeyInput", Define.HitObjectInterval);
        //}

        //if (inputIndex < noteTimeMidi.Count)
        //{
        //    double timeStamp = noteTimeMidi[inputIndex];
        //    double marginOfError = SongManager.Instance.MarginOfError;
        //    double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.InputDelayInMilliseconds / 1000.0);

        //    if (Input.GetKeyDown(KeyInput))
        //    {
        //        if (Math.Abs(audioTime - timeStamp) < marginOfError)
        //        {
        //            //Hit();
        //            Debug.LogError(String.Format("Hit on {0} note", inputIndex + 1));
        //            var temp = notes[inputIndex];
        //            Destroy(temp.gameObject);
        //            inputIndex++;
        //        }
        //        else
        //        {
        //            //Debug.LogError(String.Format("Hit inaccurate on {0} note with {1} delay", inputIndex, Math.Abs(audioTime - timeStamp)));
        //            //Debug.LogError("tre");
        //        }
        //    }

        //    if (timeStamp + marginOfError <= audioTime)
        //    {
        //        ScoreManager.Miss();
        //        inputIndex++;
        //    }
        //}
    }

    private void OnClickHitButton()
    {
        lastClickedTime = Time.time;
    }

    public void OnResponseNoteMiss(Component component, object data)
    {
        int index;
        if (data is int)
        {
            index = (int)data;
            var temp = notes[index];
            Destroy(temp.gameObject);
        }
        else
        {
            Debug.LogError("Wront data pack in OnResponseNoteMiss");
        }
    }
}
