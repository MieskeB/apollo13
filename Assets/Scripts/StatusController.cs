using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private IniFileReloader _iniFileReloader;

    // Sliders for oxygen tanks in the CM
    [SerializeField] private Slider oxygenTanksCM1;
    [SerializeField] private Slider oxygenTanksCM2;
    [SerializeField] private Slider oxygenTanksCM3;
    [SerializeField] private Slider oxygenTankLM;

    [SerializeField] private Slider fuelTankCMDisplay;
    [SerializeField] private Slider fuelTankLMDisplay;
    [SerializeField] private Slider waterCMDisplay;
    [SerializeField] private Slider waterLMDisplay;
    [SerializeField] private Slider powerCMDisplay;
    [SerializeField] private Slider powerLMDisplay;

    [SerializeField] private TextMeshProUGUI powerTextCM;
    [SerializeField] private TextMeshProUGUI powerTextLM;

    private float initialOxygenPerCMTank;
    private float initialOxygenLM;

    private float initialFuel;
    private float initialWater;
    private float initialPower;

    private float initialFuelLM;
    private float initialWaterLM;
    private float initialPowerLM;


    private float currentOxygenCM1;
    private float currentOxygenCM2;
    private float currentOxygenCM3;
    private float currentOxygenLM; // Current oxygen level for LM tank
    private float currentFuelCM;
    private float currentFuelLM;
    private float currentWaterCM;
    private float currentWaterLM;
    private float currentPowerCM;
    private float currentPowerLM;

    private float totalGameDuration;
    private float timer = 0f;
    private float updateInterval = 4f; // Update every 4 seconds

    private bool hasStarted = false;

    private float lmTurnedOnTime = -1f;

    private void Start()
    {
        totalGameDuration = gameManager.GetGameDurationSeconds();
    }

    private void Update()
    {
        if (!hasStarted && gameManager.GetActiveScreen() == GameManager.Screen.Status)
        {
            hasStarted = true;

            initialOxygenPerCMTank = this._iniFileReloader.GetStatus("oxygenCM1");
            initialFuel = this._iniFileReloader.GetStatus("fuelCM");
            initialWater = this._iniFileReloader.GetStatus("waterCM");
            initialPower = this._iniFileReloader.GetStatus("powerCM");
            
            initialOxygenLM = this._iniFileReloader.GetStatus("oxygenLM");
            initialFuelLM = this._iniFileReloader.GetStatus("fuelLM");
            initialWaterLM = this._iniFileReloader.GetStatus("waterLM");
            initialPowerLM = this._iniFileReloader.GetStatus("powerLM");

            oxygenTanksCM1.maxValue = initialOxygenPerCMTank;
            oxygenTanksCM1.value = initialOxygenPerCMTank;
            currentOxygenCM1 = initialOxygenPerCMTank;
            oxygenTanksCM2.maxValue = initialOxygenPerCMTank;
            oxygenTanksCM2.value = initialOxygenPerCMTank;
            currentOxygenCM2 = initialOxygenPerCMTank;
            oxygenTanksCM3.maxValue = initialOxygenPerCMTank;
            oxygenTanksCM3.value = initialOxygenPerCMTank;
            currentOxygenCM3 = initialOxygenPerCMTank;

            fuelTankCMDisplay.maxValue = initialFuel;
            fuelTankCMDisplay.value = initialFuel;
            currentFuelCM = initialFuel;
            waterCMDisplay.maxValue = initialWater;
            waterCMDisplay.value = initialWater;
            currentWaterCM = initialWater;
            powerCMDisplay.maxValue = initialPower;
            powerCMDisplay.value = initialPower;
            currentPowerCM = initialPower;

            oxygenTankLM.maxValue = initialOxygenLM;
            oxygenTankLM.value = initialOxygenLM;
            currentOxygenLM = initialOxygenLM;
            
            fuelTankLMDisplay.maxValue = initialFuelLM;
            fuelTankLMDisplay.value = initialFuelLM;
            currentFuelLM = initialFuelLM;
            waterLMDisplay.maxValue = initialWaterLM;
            waterLMDisplay.value = initialWaterLM;
            currentWaterLM = initialWaterLM;
            powerLMDisplay.maxValue = initialPowerLM;
            powerLMDisplay.value = initialPowerLM;
            currentPowerLM = initialPowerLM;
        }

        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;
            DepleteResources();
            if (gameManager.GetActiveScreen() == GameManager.Screen.Status)
            {
                UpdateUIElements();
            }
        }
    }

    private void DepleteResources()
    {
        float oxygenDepletionPerSecondCM = initialOxygenPerCMTank / totalGameDuration * updateInterval;
        float fuelDepletionPerSecondCM = initialFuel / totalGameDuration * updateInterval;
        float waterDepletionPerSecondCM = initialWater / totalGameDuration * updateInterval;
        float powerDepletionPerSecondCM = initialPower / totalGameDuration * updateInterval;

        if (Math.Abs(lmTurnedOnTime - (-1f)) < 1)
        {
            lmTurnedOnTime = this._iniFileReloader.GetStatus("lmStartTime");
        }
        else
        {
            float oxygenDepletionPerSecondLM = initialOxygenLM / (totalGameDuration - lmTurnedOnTime) * updateInterval;
            float fuelDepletionPerSecondLM = initialFuelLM / (totalGameDuration - lmTurnedOnTime) * updateInterval;
            float waterDepletionPerSecondLM = initialWaterLM / (totalGameDuration - lmTurnedOnTime) * updateInterval;
            float powerDepletionPerSecondLM = initialPowerLM / (totalGameDuration - lmTurnedOnTime) * updateInterval;
        
            currentOxygenLM = Mathf.Max(0, this._iniFileReloader.GetStatus("oxygenLM") - oxygenDepletionPerSecondLM);
            currentFuelLM = Mathf.Max(0, this._iniFileReloader.GetStatus("fuelLM") - fuelDepletionPerSecondLM);
            currentWaterLM = Mathf.Max(0, this._iniFileReloader.GetStatus("waterLM") - waterDepletionPerSecondLM);
            currentPowerLM = Mathf.Max(0, this._iniFileReloader.GetStatus("powerLM") - powerDepletionPerSecondLM);
            
            this._iniFileReloader.StartSaving();
        
            this._iniFileReloader.SaveStatus("oxygenLM", currentOxygenLM);
            this._iniFileReloader.SaveStatus("fuelLM", currentFuelLM);
            this._iniFileReloader.SaveStatus("waterLM", currentWaterLM);
            this._iniFileReloader.SaveStatus("powerLM", currentPowerLM);
            
            this._iniFileReloader.StopSaving();
        }

        currentOxygenCM1 = Mathf.Max(0, this._iniFileReloader.GetStatus("oxygenCM1") - oxygenDepletionPerSecondCM);
        currentOxygenCM2 = Mathf.Max(0, this._iniFileReloader.GetStatus("oxygenCM2") - oxygenDepletionPerSecondCM);
        currentOxygenCM3 = Mathf.Max(0, this._iniFileReloader.GetStatus("oxygenCM3") - oxygenDepletionPerSecondCM);
        currentFuelCM = Mathf.Max(0, this._iniFileReloader.GetStatus("fuelCM") - fuelDepletionPerSecondCM);
        currentWaterCM = Mathf.Max(0, this._iniFileReloader.GetStatus("waterCM") - waterDepletionPerSecondCM);
        currentPowerCM = Mathf.Max(0, this._iniFileReloader.GetStatus("powerCM") - powerDepletionPerSecondCM);

        this._iniFileReloader.StartSaving();
        
        this._iniFileReloader.SaveStatus("oxygenCM1", currentOxygenCM1);
        this._iniFileReloader.SaveStatus("oxygenCM2", currentOxygenCM2);
        this._iniFileReloader.SaveStatus("oxygenCM3", currentOxygenCM3);
        this._iniFileReloader.SaveStatus("fuelCM", currentFuelCM);
        this._iniFileReloader.SaveStatus("waterCM", currentWaterCM);
        this._iniFileReloader.SaveStatus("powerCM", currentPowerCM);
        
        this._iniFileReloader.StopSaving();
    }

    private void UpdateUIElements()
    {
        oxygenTanksCM1.value = currentOxygenCM1;
        oxygenTanksCM2.value = currentOxygenCM2;
        oxygenTanksCM3.value = currentOxygenCM3;
        fuelTankCMDisplay.value = currentFuelCM;
        waterCMDisplay.value = currentWaterCM;
        powerCMDisplay.value = currentPowerCM;
        
        oxygenTankLM.value = currentOxygenLM;
        fuelTankLMDisplay.value = currentFuelLM;
        waterLMDisplay.value = currentWaterLM;
        powerLMDisplay.value = currentPowerLM;

        powerTextCM.text = Mathf.Round(currentPowerCM * 10) / 10 + " Wh";
        powerTextLM.text = Mathf.Round(currentPowerLM * 10) / 10 + " Wh";
    }
}