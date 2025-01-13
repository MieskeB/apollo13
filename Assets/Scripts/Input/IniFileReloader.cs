using System.IO;
using UnityEngine;

public class IniFileReloader : MonoBehaviour
{
    private string iniFileName = "stats.ini";
    private string iniFilePath;
    private INIParser _iniParser;
    
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
        return float.Parse(this._iniParser.ReadValue(astronautName, biometric, "0"));
    }
}