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
    public RootObject fileData;

    public void Start()
    {
        // Assuming the .sln file is in the root project directory, we access it directly.
        filePath = Path.Combine(Directory.GetCurrentDirectory(), "output.json");
        Debug.Log("filePath: " + filePath);
        LoadFloorData();
    }

    void LoadFloorData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            //fileData = JsonUtility.FromJson<RootObject>(json);
            
            fileData = JsonConvert.DeserializeObject<RootObject>(json);
            Debug.Log("Floor Count: " + fileData?.files?.Count());
        }
        else
        {
            Debug.LogError("JSON file not found at " + filePath);
        }
    }

    // Getter for floor count
    public int? GetFloorCount()
    {
        return fileData?.files[0]?.classes?.Count();
    }
}
