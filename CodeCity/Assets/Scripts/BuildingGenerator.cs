using Assets.Entities;
using System;
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

    // Start method to generate the building
    public void StartCityGeneration()
    {
        jsonReader = Object.FindFirstObjectByType<JsonReader>();
        // Check if the BuildingManager was found
        if (jsonReader != null)
        {
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
            //for (int z = 0; z < rows && currentBuilding < count; z++)
            //{
            // Calculate the position of each building
            float xPosition = (x % 2 == 0) ? x * (cubeSize.x + offset) : -x * (cubeSize.x + offset);
            Vector3 position = new Vector3(xPosition, 0, x * (cubeSize.z) );
            LogFileWriter.WriteLog($"Building Position: x={position.x} y={position.y} z={position.z}");

            // Generate a building at the calculated position
            GenerateBuilding(building, position);

                buildingIndex++;
            //    break;
            //}
        }
    }

    // Method to generate the building with given number of stories
    void GenerateBuilding(File FileObject, Vector3 position)
    {
        int stories = FileObject?.classes?.Length ?? 0;
        if (stories <= 0)//TO DO: need to change this logic when functions without classes are displayed
        {
            return;
        }
        GameObject building = new GameObject("Building");
        // AddBuildingLabel(building, position, FileObject.name);
        AddBuildingLabel(building, position, FileObject.name);
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        LogFileWriter.WriteLog(building.name, "Floor count", stories);
        for (int floorIndx = 0; floorIndx < stories; floorIndx++)
        {
            Class clasObject = FileObject.classes[floorIndx];
            LogFileWriter.WriteLog("rendering floor", floorIndx);
            // Create a new GameObject for each floor
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Set the size of the floor
            floor.transform.localScale = cubeSize;

            // Position the floor at the correct height
            Vector3 floorPosition = position + new Vector3(0, floorIndx * (cubeSize.y + spacing), 0);
            floor.transform.position = floorPosition;
            //set color
            Renderer floorRenderer = floor.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
                floorRenderer.material.color = randomColor;
            }
            // Parent the floor to the building
            floor.transform.parent = building.transform;

            // Add a label to the floor
            //AddFloorLabel(floor, clasObject.name, floorIndx);
            AddFloorLabel1(floor, floorPosition, clasObject.name, floorIndx);
        }

        // Set the parent of the building to this GameObject for organization
        building.transform.parent = this.transform;

       
    }
    
    void AddBuildingLabel(GameObject building, Vector3 buildingPosition, string name)
    {
        // Create a new GameObject for the TextMespositionhPro text
        GameObject textObject = new GameObject("FloorLabel");
        textObject.transform.position = buildingPosition;

        // Add a TextMeshPro component to the object
        TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();

        // Set the text properties
        textMeshPro.font = fontAsset;
        textMeshPro.text = name;
        textMeshPro.fontSize = 8;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.black;

        Vector3 labelPosition = new Vector3( buildingPosition.x, buildingPosition.y - cubeSize.y,  buildingPosition.z);
        textObject.transform.position = labelPosition;
        LogFileWriter.WriteLog($"Building label position: x={labelPosition.x} y={labelPosition.y} z={labelPosition.z}");

        // Assign the "TextMesh Pro - Mobile - Distance Field Overlay" shader
        textMeshPro.fontMaterial.shader = Shader.Find("TextMeshPro/Mobile/Distance Field Overlay");
        textObject.transform.SetParent(building.transform, false);
    }
    
    void AddFloorLabel1(GameObject floor, Vector3 position, string floorName, int floorIndex)
    {
        // Create a new GameObject for the TextMeshPro text
        GameObject textObject = new GameObject("AlwaysOnTopText");
        textObject.transform.position = position;

        // Add a TextMeshPro component to the object
        TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();

        // Set the text properties
        textMeshPro.text = floorName;
        textMeshPro.fontSize = 8;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.black;

        Vector3 labelPosition = new Vector3(position.x, position.y + spacing * floorIndex, position.z);
        textObject.transform.position = labelPosition;
        LogFileWriter.WriteLog($"Building label position: x={labelPosition.x} y={labelPosition.y} z={labelPosition.z}");

        // Assign the "TextMesh Pro - Mobile - Distance Field Overlay" shader
        textMeshPro.fontMaterial.shader = Shader.Find("TextMeshPro/Mobile/Distance Field Overlay");
    }

}


