using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

public class AppBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {  
        try
        {
            LogFileWriter.CreateLog();
            JsonReader jsonReader = FindFirstObjectByType<JsonReader>();
            jsonReader.StartReadingJson();
            BuildingGenerator buildingGenerator = FindFirstObjectByType<BuildingGenerator>();
            buildingGenerator.StartCityGeneration();
        }
        catch (Exception e)
        {
            LogFileWriter.WriteLog("Error Occures");
            LogFileWriter.WriteLog(e.ToString());
        }

    }
}
