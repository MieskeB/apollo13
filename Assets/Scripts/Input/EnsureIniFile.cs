using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnsureIniFile : MonoBehaviour
{
    private string iniFileName = "stats.ini";
    private string iniFilePath;

    private void Start()
    {
        iniFilePath = Path.Combine(Application.persistentDataPath, iniFileName);
        if (!File.Exists(iniFilePath))
        {
            CopyFileToPersistentDataPath();
        }
        
        Debug.Log($"INI file path: {iniFilePath}");
    }
    
    private void CopyFileToPersistentDataPath()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, iniFileName);

        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                // For Android, StreamingAssets is compressed in APK and needs special handling
                StartCoroutine(CopyFileFromStreamingAssetsAndroid(sourcePath));
            }
            else
            {
                // Copy file directly for other platforms
                File.Copy(sourcePath, iniFilePath);
                Debug.Log($"INI file copied to: {iniFilePath}");
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error copying INI file: {e.Message}");
        }
    }
    
    private System.Collections.IEnumerator CopyFileFromStreamingAssetsAndroid(string sourcePath)
    {
        WWW reader = new WWW(sourcePath);
        yield return reader;

        if (string.IsNullOrEmpty(reader.error))
        {
            File.WriteAllBytes(iniFilePath, reader.bytes);
            Debug.Log($"INI file copied to: {iniFilePath}");
        }
        else
        {
            Debug.LogError($"Error accessing StreamingAssets on Android: {reader.error}");
        }
    }
}
