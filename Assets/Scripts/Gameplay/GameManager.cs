using System;
using System.Collections;
using System.Collections.Generic;
using Core.Event;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Events")]
    public EventChannel playerTap;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerTap.Raise(this, 1);
        }
    }
}
