using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Note : MonoBehaviour
{
    [Header("Events")]
    public GameEvent OnNoteMiss;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 hitPos;
    private double timeInstantiated;

    public Vector3 EndPos
    {
        get => endPos;
        set => endPos = value;
    }

    public Vector3 StartPos
    {
        get => startPos;
        set => startPos = value;
    }

    public Vector3 HitPos
    {
        get => hitPos;
        set => hitPos = value;
    }
    private void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    private void Update()
    {
        if (gameObject != null)
        {
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
            float t = (float)(timeSinceInstantiated / SongManager.Instance.NoteTime);

            if (t > 1)
            {
                OnNoteMiss.Invoke(null, null);
                float t2 = t - 1;
                if (t2 > 1)
                {
                    OnNoteMiss.Invoke(null, null);
                }
                else
                {
                    transform.position = Vector3.Lerp(hitPos, endPos, t2);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(startPos, hitPos, t);
            }
        }
    }
    public void OnFinishNote()
    {
        Destroy(gameObject);
    }
}