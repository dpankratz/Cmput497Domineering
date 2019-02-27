using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager : MonoBehaviour
{
    public static SerializationManager instance;

    private readonly String FileName = "Settings";
    private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

    private void Awake()
    {
        instance = this;
        if (File.Exists(GetSettingsPath()))
        {
            Deserialize();
        }        
    }

    internal void Serialize()
    {
        var settingsFile = new FileStream(GetSettingsPath(), FileMode.OpenOrCreate);
        _binaryFormatter.Serialize(settingsFile,Settings.AgentOne);
        _binaryFormatter.Serialize(settingsFile, Settings.AgentTwo);
        settingsFile.Close();
    }

    internal void Deserialize()
    {
        var settingsFile = new FileStream(GetSettingsPath(), FileMode.Open);
        Settings.AgentOne = (AgentType)_binaryFormatter.Deserialize(settingsFile);
        Settings.AgentTwo = (AgentType)_binaryFormatter.Deserialize(settingsFile);
        settingsFile.Close();
    }

    private string GetSettingsPath()
    {
        return Application.persistentDataPath + "/" + FileName;
    }
}
