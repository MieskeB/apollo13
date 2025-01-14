using System;
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
    [SerializeField] private Slider waterDisplay;
    [SerializeField] private Slider powerDisplay;

    private float initialOxygenPerCMTank;
    private float initialOxygenLM;

    private float initialFuel;
    private float initialWater;
    private float initialPower;


    private float currentOxygenCM1;
    private float currentOxygenCM2;
    private float currentOxygenCM3;
    private float currentOxygenLM; // Current oxygen level for LM tank
    private float currentFuel;
    private float currentWater;
    private float currentPower;

    private float totalGameDuration;
    private float timer = 0f;
    private float updateInterval = 3f; // Update every 3 seconds

    private bool hasStarted = false;

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
            initialOxygenLM = this._iniFileReloader.GetStatus("oxygenLM");
            initialFuel = this._iniFileReloader.GetStatus("fuelCM");
            initialWater = this._iniFileReloader.GetStatus("water");
            initialPower = this._iniFileReloader.GetStatus("power");

            oxygenTanksCM1.maxValue = initialOxygenPerCMTank;
            oxygenTanksCM1.value = initialOxygenPerCMTank;
            currentOxygenCM1 = initialOxygenPerCMTank;
            oxygenTanksCM2.maxValue = initialOxygenPerCMTank;
            oxygenTanksCM2.value = initialOxygenPerCMTank;
            currentOxygenCM2 = initialOxygenPerCMTank;
            oxygenTanksCM3.maxValue = initialOxygenPerCMTank;
            oxygenTanksCM3.value = initialOxygenPerCMTank;
            currentOxygenCM3 = initialOxygenPerCMTank;

            // Initialize slider for LM tank
            oxygenTankLM.maxValue = initialOxygenLM;
            oxygenTankLM.value = initialOxygenLM;
            currentOxygenLM = initialOxygenLM;

            // Initialize other resource displays
            fuelTankCMDisplay.maxValue = initialFuel;
            fuelTankCMDisplay.value = initialFuel;
            waterDisplay.maxValue = initialWater;
            waterDisplay.value = initialWater;
            powerDisplay.maxValue = initialPower;
            powerDisplay.value = initialPower;

            currentFuel = initialFuel;
            currentWater = initialWater;
            currentPower = initialPower;
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
        float oxygenDepletionPerSecondLM = initialOxygenLM / totalGameDuration * updateInterval;
        float fuelDepletionPerSecond = initialFuel / totalGameDuration * updateInterval;
        float waterDepletionPerSecond = initialWater / totalGameDuration * updateInterval;
        float powerDepletionPerSecond = initialPower / totalGameDuration * updateInterval;

        currentOxygenCM1 = Mathf.Max(0, this._iniFileReloader.GetStatus("oxygenCM1") - oxygenDepletionPerSecondCM);
        currentOxygenCM2 = Mathf.Max(0, this._iniFileReloader.GetStatus("oxygenCM2") - oxygenDepletionPerSecondCM);
        currentOxygenCM3 = Mathf.Max(0, this._iniFileReloader.GetStatus("oxygenCM3") - oxygenDepletionPerSecondCM);
        currentOxygenLM = Mathf.Max(0, this._iniFileReloader.GetStatus("oxygenLM") - oxygenDepletionPerSecondLM);
        currentFuel = Mathf.Max(0, this._iniFileReloader.GetStatus("fuelCM") - fuelDepletionPerSecond);
        currentWater = Mathf.Max(0, this._iniFileReloader.GetStatus("water") - waterDepletionPerSecond);
        currentPower = Mathf.Max(0, this._iniFileReloader.GetStatus("power") - powerDepletionPerSecond);

        this._iniFileReloader.StartSaving();
        this._iniFileReloader.SaveStatus("oxygenCM1", currentOxygenCM1);
        this._iniFileReloader.SaveStatus("oxygenCM2", currentOxygenCM2);
        this._iniFileReloader.SaveStatus("oxygenCM3", currentOxygenCM3);
        this._iniFileReloader.SaveStatus("oxygenLM", currentOxygenLM);
        this._iniFileReloader.SaveStatus("fuelCM", currentFuel);
        this._iniFileReloader.SaveStatus("water", currentWater);
        this._iniFileReloader.SaveStatus("power", currentPower);
        this._iniFileReloader.StopSaving();
    }

    private void UpdateUIElements()
    {
        oxygenTanksCM1.value = currentOxygenCM1;
        oxygenTanksCM2.value = currentOxygenCM2;
        oxygenTanksCM3.value = currentOxygenCM3;
        oxygenTankLM.value = currentOxygenLM;
        fuelTankCMDisplay.value = currentFuel;
        waterDisplay.value = currentWater;
        powerDisplay.value = currentPower;
    }
}