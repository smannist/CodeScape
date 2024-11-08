using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //FindObjectOfType<JsonReader>();
        LogFileWriter.CreateLog();
        JsonReader jsonReader = FindObjectOfType<JsonReader>();
        jsonReader.StartReadingJson();
        BuildingGenerator buildingGenerator = FindObjectOfType<BuildingGenerator>();
        buildingGenerator.StartCityGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
