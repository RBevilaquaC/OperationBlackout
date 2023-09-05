using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//C:\Users\rober\AppData\LocalLow\DefaultCompany\Operation Blackout

public static class SaveSystem
{
    public static string path = Application.persistentDataPath + "/saveOpBlackout.star";
    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData data = new GameData();
    
        formatter.Serialize(stream,data);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
    
        Debug.LogError("Nenhum save encontrado no endereço: " + path);
        return null;
    }
}
