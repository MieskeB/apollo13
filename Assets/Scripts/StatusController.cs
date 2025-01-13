using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI oxygenTankText;
    [SerializeField] private TextMeshProUGUI fuelTankText;
    [SerializeField] private TextMeshProUGUI waterText;
    [SerializeField] private TextMeshProUGUI powerText;
    
    private float timer = 0f;
    private const float UpdateInterval = 1f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= UpdateInterval)
        {
            timer = 0f;
        }
    }
}
