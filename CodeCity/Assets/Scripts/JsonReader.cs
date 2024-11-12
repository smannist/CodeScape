using Assets.Entities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using static BuildingGenerator;
using File = System.IO.File;
//using SourceFile = Assets.Entities.File;

public class JsonReader : MonoBehaviour
{
    private string filePath;
    public FileData fileData;

    public void StartReadingJson()
    {
        // Assuming the .sln file is in the root project directory, we access it directly.
        filePath = Path.Combine(Directory.GetCurrentDirectory(), "output.json");
        LogFileWriter.WriteLog("filePath:" , filePath);
        LoadFileData();
    }

    private void LoadFileData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            //fileData = JsonUtility.FromJson<RootObject>(json);

            fileData = JsonConvert.DeserializeObject<FileData>(json);
            Debug.Log("Floor Count: " + fileData?.files?.Count());
        }
        else
        {
            Debug.LogError("JSON file not found at " + filePath);
        }
    }

}
