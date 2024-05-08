using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Note : MonoBehaviour
{
    //[Header("Events")]
    //public GameEvent onNoteInActivator;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private Vector3 _hitPos;
    private double _timeInstantiated;
    private bool _isHit = false;

    public Vector3 EndPos
    {
        get => _endPos;
        set => _endPos = value;
    }

    public Vector3 StartPos
    {
        get => _startPos;
        set => _startPos = value;
    }

    public Vector3 HitPos
    {
        get => _hitPos;
        set => _hitPos = value;
    }
    private void Start()
    {
        _timeInstantiated = SongManager.GetAudioSourceTime();
    }

    private void Update()
    {
        if (gameObject != null)
        {
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - _timeInstantiated;
            float t = (float)(timeSinceInstantiated / SongManager.Instance.NoteTime);

            if (t > 1)
            {
                float t2 = t - 1;
                if (t2 > 1)
                {
                    OnFinishNote();
                }
                else
                {
                    transform.position = Vector3.Lerp(_hitPos, _endPos, t2);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(_startPos, _hitPos, t);
            }
        }
    }
    public void OnFinishNote()
    {
        Debug.LogError("Note Finish");
        Destroy(gameObject);
    }
}