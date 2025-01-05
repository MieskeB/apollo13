using UnityEngine;
using System.IO;
using TMPro;

public class InputManager : MonoBehaviour
{
    private string filePath;
    private readonly float checkInterval = 5.0f;
    private float nextCheckTime = 0f;

    private void Start()
    {
        filePath = Path.Combine(Application.dataPath, "GameData.txt");
        if (!File.Exists(filePath))
        {
            WriteToFile("Initial game data.");
        }
    }

    private void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            // DisplayFileContents();
        }
    }

    private void WriteToFile(string data)
    {
        File.WriteAllText(filePath, data);
    }

    private string ReadFromFile()
    {
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        return "File not found.";
    }
}