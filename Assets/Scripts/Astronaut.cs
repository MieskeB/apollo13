using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Astronaut : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI heartBeatText;
    [SerializeField] private TextMeshProUGUI bloodPressureText;
    [SerializeField] private TextMeshProUGUI respiratoryRateText;
    [SerializeField] private TextMeshProUGUI bodyTemperatureText;
    [SerializeField] private TextMeshProUGUI oxygenSaturationText;
    [SerializeField] private TextMeshProUGUI co2LevelText;

    private float _heartBeat = 80f;
    private float _bloodPressureUpper = 120f;
    private float _bloodPressureLower = 80f;
    private float _respiratoryRate = 14f;
    private float _bodyTemperature = 37.0f;
    private float _oxygenSaturation = 97f;
    private float _co2Level = 3f;

    private float timer = 0f;
    private const float UpdateInterval = 1f;

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= UpdateInterval)
        {
            timer = 0f;
            UpdateHeartBeatText();
            UpdateBloodPressureText();
            UpdateRespiratoryRateText();
            UpdateBodyTemperatureText();
            UpdateOxygenSaturationText();
            UpdateCo2LevelText();
        }
    }

    public void SetHeartBeat(float heartBeat)
    {
        this._heartBeat = heartBeat;
        this.UpdateHeartBeatText();
    }

    private void UpdateHeartBeatText()
    {
        float fluctuatedHeartBeat = this._heartBeat + UnityEngine.Random.Range(-5, 6);
        this.heartBeatText.text = "Heart beat rate: " + fluctuatedHeartBeat + " bpm";
    }

    public void SetBloodPressure(float bloodPressureUpper, float bloodPressureLower)
    {
        this._bloodPressureUpper = bloodPressureUpper;
        this._bloodPressureLower = bloodPressureLower;
        this.UpdateBloodPressureText();
    }

    private void UpdateBloodPressureText()
    {
        this.bloodPressureText.text = "Blood pressure: " + this._bloodPressureUpper + " / " + this._bloodPressureLower + " mmHg";
    }

    public void SetRespiratoryRate(float respiratoryRate)
    {
        this._respiratoryRate = respiratoryRate;
        this.UpdateRespiratoryRateText();
    }

    private void UpdateRespiratoryRateText()
    {
        float fluctuatedRespiratoryRate = this._respiratoryRate + UnityEngine.Random.Range(-1, 1);
        this.respiratoryRateText.text = "Respiratory rate: " + fluctuatedRespiratoryRate + " bpm";
    }
    
    public void SetBodyTemperature(float bodyTemperature)
    {
        this._bodyTemperature = bodyTemperature;
        this.UpdateBodyTemperatureText();
    }

    private void UpdateBodyTemperatureText()
    {
        float fluctuatedBodyTemperature = (float)Math.Round(this._bodyTemperature + UnityEngine.Random.Range(-0.3f, 0.3f), 1);
        this.bodyTemperatureText.text = "Body temperature: " + fluctuatedBodyTemperature + " \u00b0C";
    }
    
    public void SetOxygenSaturation(float oxygenSaturation)
    {
        this._oxygenSaturation = oxygenSaturation;
        this.UpdateOxygenSaturationText();
    }

    private void UpdateOxygenSaturationText()
    {
        float fluctuatedOxygenSaturation = this._oxygenSaturation + UnityEngine.Random.Range(-1, 1);
        this.oxygenSaturationText.text = "Oxygen saturation: " + fluctuatedOxygenSaturation + " %";
    }
    
    public void SetCo2Level(float co2Level)
    {
        this._co2Level = co2Level;
        this.UpdateCo2LevelText();
    }

    private void UpdateCo2LevelText()
    {
        float fluctuatedCo2Level = (float)Math.Round(this._co2Level + UnityEngine.Random.Range(-0.3f, 0.3f), 3);
        this.co2LevelText.text = "CO2 level: " + fluctuatedCo2Level + " mmHg";
    }
}
