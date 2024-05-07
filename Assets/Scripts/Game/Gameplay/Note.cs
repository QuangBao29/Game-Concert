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

    private bool _isPerfectScore = false;
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
        if (gameObject != null && gameObject.name != Define.PrefabName.NotePrefab.ToString())
        {
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - _timeInstantiated;
            float t = (float)(timeSinceInstantiated / SongManager.Instance.NoteTime);

            if (t > 1)
            {
                float t2 = t - 1;
                if (t2 > 1)
                {
                    OnFinishNotes();
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

            double lowerBound = SongManager.Instance.NoteTime - SongManager.Instance.MarginOfError;
            double upperBound = SongManager.Instance.NoteTime + SongManager.Instance.MarginOfError;
            if (timeSinceInstantiated >= lowerBound && timeSinceInstantiated <= upperBound)
            {
                _isPerfectScore = true;
            }
            else
            {
                _isPerfectScore = false;
            }
            //if (Input.GetKeyDown(InputManager.KeyInput) && !_isHit)
            //{
            //    if (_inActivator && !_inPerfectHit)
            //    {
            //        Debug.LogError("Normal Hit");
            //        NoteManager.Instance.OnNormalHit();
            //        _isHit = true;
            //        OnFinishNotes();
            //    }
            //    else if (_inPerfectHit)
            //    {
            //        Debug.LogError("Perfect Hit");
            //        NoteManager.Instance.OnPerfectHit();
            //        _isHit = true;
            //        OnFinishNotes();
            //    }
            //    else if (!_inActivator && !_inPerfectHit)
            //    {
            //        NoteManager.Instance.OnMissHit();
            //        Debug.LogError("Missed Click");
            //        Destroy(gameObject);
            //    }
            //}
        }
    }

    private void OnFinishNotes()
    {
        if (!_isHit)
        {
            //Debug.LogError("Missed!");
        }
        Destroy(gameObject);
    }
}