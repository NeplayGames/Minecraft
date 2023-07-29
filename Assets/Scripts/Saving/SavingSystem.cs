using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    private string SavePath;
    public SaveData LoadFile()
    {
        SavePath = Application.persistentDataPath + "/save.txt";
        if (!File.Exists(SavePath))
            return null;
        using (var stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (SaveData)formatter.Deserialize(stream);
        }
    }

    public void SaveFile(SaveData saveData)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, saveData);
        }
    }
}

