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
    private NoteManager _noteManager = null;
    [SerializeField]
    private GameObject _endZone = null;
    [SerializeField]
    private GameObject _startZone = null;
    [SerializeField]
    private GameObject _hitZone = null;
    [SerializeField]
    private MeshRenderer _meshRenderer = null;

    private List<Double> _spawnedTimes = new(); //timestamp that note spawned (based on midi)
    private List<Note> notes = new();
    private List<NoteName> _pitches = new();
    private int spawnIndex = 0;
    private int inputIndex = 0;
    private int _zoneIndex = 0;
    private float cooldown = Define.HitObjectInterval;
    private float lastClickedTime = 0;

    private Vector3 _originalHitPos = new Vector3(0, 0, 0);
    private bool _isClicked = false;


    /// <summary>
    /// from 0 to 3
    /// </summary>
    public int ZoneIndex
    {
        get => _zoneIndex;
        set => _zoneIndex = value;
    }
    public List<NoteName> Pitches
    {
        get => _pitches;
        set => _pitches = value;
    }
    public List<Double> SpawnedTimes
    {
        get => _spawnedTimes;
        set => _spawnedTimes = value;
    }
    public GameObject StartZone
    {
        get => _startZone;
    }
    public GameObject EndZone
    {
        get => _endZone;
    }
    private void Start()
    {
        _originalHitPos = _hitZone.transform.position;
    }
    void Update()
    {
        if (spawnIndex < _spawnedTimes.Count)
        {
            //spawn note truoc 1 khoang thoi gian NoteTime
            if (SongManager.GetAudioSourceTime() >= _spawnedTimes[spawnIndex] - SongManager.Instance.NoteTime)
            {
                var note = _noteManager.OnSpawnNotesToTarget(_startZone.transform.position, _endZone.transform.position, _hitZone.transform.position);
                notes.Add(note);
                spawnIndex++;
            }
        }

        if (Input.GetKeyDown(KeyInput) && Time.time - lastClickedTime > cooldown)
        {
            OnClickDownKeyInput();
            //check note

        }
        if (_hitZone.transform.position != _originalHitPos && _isClicked)
        {
            Invoke("OnClickUpKeyInput", Define.HitObjectInterval);
        }

        //if (inputIndex < _spawnedTimes.Count)
        //{
        //    double timeStamp = _spawnedTimes[inputIndex];
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

    private void OnClickDownKeyInput()
    {
        lastClickedTime = Time.time;
        _hitZone.transform.position = new Vector3(_originalHitPos.x, _originalHitPos.y - 0.4f, _originalHitPos.z);
        _meshRenderer.material = ActivatorManager.Instance.HitMaterial;
        _isClicked = true;
    }

    private void OnClickUpKeyInput()
    {
        _hitZone.transform.position = _originalHitPos;
        _meshRenderer.material = ActivatorManager.Instance.DefaultMaterial;
        _isClicked = false;
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
