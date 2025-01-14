using System.IO;
using UnityEngine;

public class IniFileReloader : MonoBehaviour
{
    private string iniFileName = "stats.ini";
    private string iniFilePath;
    private INIParser _iniParser;

    private bool isSaving = false;
    
    private void Start()
    {
        this._iniParser = new INIParser();
        iniFilePath = Path.Combine(Application.persistentDataPath, iniFileName);
        // while (!File.Exists(iniFilePath)) {}
        _iniParser.Open(iniFilePath);
    }

    public float GetAstronautHealth(string astronautName, string biometric)
    {
        this._iniParser.Open(iniFilePath);
        float res = float.Parse(this._iniParser.ReadValue(astronautName, biometric, "0"));
        this._iniParser.Close();
        return res;
    }

    public float GetStatus(string status)
    {
        if (this._iniParser == null)
        {
            return 0f;
        }
        this._iniParser.Open(iniFilePath);
        float res = float.Parse(this._iniParser.ReadValue("Status", status, "0"));
        this._iniParser.Close();
        return res;
    }

    public void StartSaving()
    {
        this._iniParser.Open(iniFilePath);
        isSaving = true;
    }
    
    public void SaveStatus(string status, float value)
    {
        if (!isSaving) return;
        this._iniParser.WriteValue("Status", status, value);
    }

    public void StopSaving()
    {
        this._iniParser.Close();
        isSaving = false;
    }
}