using Assets.Entities;
//using Microsoft.MixedReality.Toolkit.Build.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BuildingGenerator : MonoBehaviour
{
    private JsonReader jsonReader;
    private TMP_FontAsset fontAsset;
    // Number of stories (floors)
    public int numberOfStories = 5;
    // Size of each cube (floor)
    public Vector3 cubeSize = new Vector3(5, 1, 5);
    // Spacing between each floor
    public float spacing = 0.1f;
    public float offset = 0.9f;
    public int numberOfBuildings = 3;
    // Spacing between buildings
    public Vector3 buildingSpacing = new Vector3(10, 0, 10);

    public Shader fontShader;
    public GameObject floorPrefab;
    public GameObject funcFloorPrefab;

    // Start method to generate the building
    public void StartCityGeneration()
    {
    	StartCoroutine(JsonToCity());	
    }
    
    IEnumerator JsonToCity()
    {
        jsonReader = Object.FindFirstObjectByType<JsonReader>();
        // Check if the BuildingManager was found
        if (jsonReader != null)
        {
        
        	Debug.Log("Waiting for file load!");
        	yield return new WaitUntil(() => jsonReader.fileLoaded);
        	Debug.Log("File load done!");
        	
            FileData fileData = jsonReader.fileData;

            //load font
            fontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            // Assign the font to TextMeshPro if found
            if (fontAsset == null)
            {
                LogFileWriter.WriteLog("Font asset LiberationSans SDF not found in Resources folder.");
            }

            GenerateCity(fileData);
            //((MeshRenderer)transform.Find("TextLabel").gameObject.GetComponent("MeshRenderer")).sortingOrder = 5;
        }
        else
        {
            Debug.LogError("BuildingManager not found in the scene.");
        }

    }

   
    void GenerateCity(FileData fileData)
    {
        int buildingCount = fileData?.files?.Length ?? 0;
        int rows = Mathf.CeilToInt(Mathf.Sqrt(buildingCount)); // Number of rows and columns
        LogFileWriter.WriteLog("Building Count",buildingCount);
        int buildingIndex = 0;

        for (int x = 0; buildingIndex < buildingCount; x++)
        {
            File building = fileData.files[buildingIndex];
            LogFileWriter.WriteLog("Building", buildingIndex, "name", building.name);
            // Calculate the position of each building
            float xPosition = (x % 2 == 0) ? x * (cubeSize.x + offset) : -x * (cubeSize.x + offset);
            Vector3 position = new Vector3(xPosition, 0, x * (cubeSize.z) );
            LogFileWriter.WriteLog($"Building Position: x={position.x} y={position.y} z={position.z}");

            // Generate a building at the calculated position
            GenerateBuilding(building, position);
            buildingIndex++;
        }
    }

    // Method to generate the building with given number of stories
    void GenerateBuilding(File FileObject, Vector3 position)
    {
        Class[] classes = FileObject?.classes;
        Function[] functions = FileObject?.functions;
        int stories = classes?.Length ?? 0;
        LogFileWriter.WriteLog(FileObject.name, "Floor count", stories);

        if (stories <= 0 && functions.Length == 0)
        {
            return;
        }
        GameObject building = new GameObject("Building");
        building.tag = "Building";
        AddBuildingLabel(building, position, FileObject.name);
        GenerateFloors(building, position, FileObject);
        
        // Set the parent of the building to this GameObject for organization
        building.transform.parent = this.transform;
    }

    void GenerateFloors(GameObject building, Vector3 buildingPosition, File fileobj)
    {
        Class[] classes = fileobj?.classes;
        Color buildingColor = new Color(Random.value, Random.value, Random.value);
        int floorCount = classes?.Length ?? 0;
        for (int floorIndx = 0; floorIndx < floorCount; floorIndx++){
            Vector3 floorPosition = buildingPosition + new Vector3(0, floorIndx * (cubeSize.y + spacing), 0);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 20, 0)); // Set rotation (e.g., 45 degrees on the Y-axis)
            GameObject floor = Instantiate(floorPrefab, floorPosition, rotation, building.transform);
            
            floor.GetComponent<Floor>().classObj = classes[floorIndx];
            floor.transform.localScale = cubeSize;
            SetObjectColor(floor, buildingColor);
        }
        
        // Function floor
        if(fileobj?.functions.Length > 0){
            Vector3 floorPosition = buildingPosition + new Vector3(0, floorCount * (cubeSize.y + spacing), 0);
            GameObject floor = Instantiate(funcFloorPrefab, floorPosition, Quaternion.identity, building.transform);
            SetObjectColor(floor, buildingColor);
            floor.GetComponent<FunctionFloor>().functions = fileobj.functions;
            floor.GetComponent<FunctionFloor>().filename = fileobj.name;
            floor.GetComponent<FunctionFloor>().fileOverview = fileobj.overview;
        }
    }

    void SetObjectColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    void AddBuildingLabel(GameObject building, Vector3 buildingPosition, string name)
    {
        // Create a new GameObject for the TextMespositionhPro text
        GameObject textObject = new GameObject("FloorLabel");

        // Add a TextMeshPro component to the object
        TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();

        // Set the text properties
        textMeshPro.font = fontAsset;
        textMeshPro.text = name;
        textMeshPro.fontSize = 8;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.black;

        Vector3 labelPosition = new Vector3( buildingPosition.x, buildingPosition.y - cubeSize.y,  buildingPosition.z) - 2*(floorPrefab.transform.forward-floorPrefab.transform.right);
        textObject.transform.position = labelPosition;
        LogFileWriter.WriteLog($"Building label position: x={labelPosition.x} y={labelPosition.y} z={labelPosition.z}");

        // Assign the "TextMesh Pro - Mobile - Distance Field Overlay" shader
        textMeshPro.fontMaterial.shader = fontShader;
        textObject.transform.SetParent(building.transform, false);
    }
    


}


