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

    [SerializeField]
    private NoteManager noteManager = null;
    [SerializeField]
    private GameObject endZone = null;
    [SerializeField]
    private GameObject startZone = null;
    [SerializeField]
    private GameObject hitZone = null;
    //[SerializeField]
    //private MeshRenderer _meshRenderer = null;

    private List<Double> spawnedTimes = new(); //timestamp that note spawned (based on midi)
    private List<Note> notes = new();
    private List<NoteName> pitches = new();
    private int spawnIndex = 0;
    private int inputIndex = 0;
    private int zoneIndex = 0;
    private float cooldown = Define.HitObjectInterval;
    private float lastClickedTime = 0;

    //private Vector3 originalHitPos = new Vector3(0, 0, 0);
    private bool isClicked = false;


    /// <summary>
    /// from 0 to 3
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
    public List<Double> SpawnedTimes
    {
        get => spawnedTimes;
        set => spawnedTimes = value;
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
        if (spawnIndex < spawnedTimes.Count)
        {
            //spawn note truoc 1 khoang thoi gian NoteTime
            if (SongManager.GetAudioSourceTime() >= spawnedTimes[spawnIndex] - SongManager.Instance.NoteTime)
            {
                var note = noteManager.OnSpawnNotesToTarget(startZone.transform.position, endZone.transform.position, hitZone.transform.position);
                notes.Add(note);
                spawnIndex++;
            }
        }

        if (Input.GetKeyDown(KeyInput) && Time.time - lastClickedTime > cooldown)
        {
            OnClickHitButton();
            //check note

        }
        //if (hitZone.transform.position != originalHitPos && isClicked)
        //{
        //    Invoke("OnClickUpKeyInput", Define.HitObjectInterval);
        //}

        //if (inputIndex < spawnedTimes.Count)
        //{
        //    double timeStamp = spawnedTimes[inputIndex];
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
        //hitZone.transform.position = new Vector3(originalHitPos.x, originalHitPos.y - 0.4f, originalHitPos.z);
        isClicked = true;
    }

    public void OnResponseNoteMiss(Component component, object data)
    {
        int index;
        if (data is int)
        {
            index = (int)data;
            var temp = notes[index];
            Destroy(temp.gameObject);
            ScoreManager.Miss();
        }
        else
        {
            Debug.LogError("Wront data pack in OnResponseNoteMiss");
        }
    }
}
