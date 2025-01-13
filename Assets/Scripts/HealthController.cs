using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private List<Astronaut> astronauts;
    [SerializeField] private IniFileReloader _iniFileReloader;

    private float timer = 0f;
    private const float UpdateInterval = 1f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= UpdateInterval)
        {
            timer = 0f;
            
            foreach (Astronaut astronaut in astronauts)
            {
                astronaut.SetHeartBeat(this._iniFileReloader.GetAstronautHealth(astronaut.getName(), "heartBeat"));
                astronaut.SetBloodPressure(
                    this._iniFileReloader.GetAstronautHealth(astronaut.getName(), "bloodPressureUpper"),
                    this._iniFileReloader.GetAstronautHealth(astronaut.getName(), "bloodPressureLower"));
                astronaut.SetRespiratoryRate(
                    this._iniFileReloader.GetAstronautHealth(astronaut.getName(), "respiratoryRate"));
                astronaut.SetBodyTemperature(
                    this._iniFileReloader.GetAstronautHealth(astronaut.getName(), "bodyTemperature"));
                astronaut.SetOxygenSaturation(
                    this._iniFileReloader.GetAstronautHealth(astronaut.getName(), "oxygenSaturation"));
                astronaut.SetCo2Level(this._iniFileReloader.GetAstronautHealth(astronaut.getName(), "co2Level"));
            }
        }
    }
}
