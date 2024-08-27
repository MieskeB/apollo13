using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Description("Total game time after launch in milliseconds")]
    private float _totalTime;
    [Description("Current game time after launch in milliseconds")]
    private float _currentTime;


    private void Start()
    {
        // 1 hour in milliseconds
        _totalTime = 1 * 60 * 60 * 1000;
    }


    private void Update()
    {
        _currentTime += Time.deltaTime;
    }


    public float GetCurrentTime()
    {
        return _currentTime;
    }
}
