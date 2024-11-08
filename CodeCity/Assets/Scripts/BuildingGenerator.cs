using Assets.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
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
        jsonReader = FindObjectOfType<JsonReader>();
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
            floor.transform.position = position + new Vector3(0, floorIndx * (cubeSize.y + spacing), 0);
            //set color
            Renderer floorRenderer = floor.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
                floorRenderer.material.color = randomColor;
            }
            // Parent the floor to the building
            floor.transform.parent = building.transform;

            // Add a label to the floor
            AddFloorLabel(floor, clasObject.name, floorIndx);
        }

        // Set the parent of the building to this GameObject for organization
        building.transform.parent = this.transform;

       
    }
    void AddFloorLabel(GameObject floor, string floorName, int floorNumber)
    {
        GameObject textObject = new GameObject("TextLabel");
        TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();
        // Ensure a font asset is assigned
        textMeshPro.font = fontAsset;

        // Set the label's text and appearance
        textMeshPro.text = name;
        textMeshPro.fontSize = 8;
        textMeshPro.color = Color.black;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        Vector3 labelPosition = new Vector3(floor.transform.position.x - cubeSize.x / 2, floor.transform.position.y + cubeSize.y / 2 * floorNumber, floor.transform.position.z + offset);
        LogFileWriter.WriteLog($"Floor label position: x={labelPosition.x} y={labelPosition.y} z={labelPosition.z}");
        // Set the label's position
        textObject.transform.position = labelPosition;

        // Vector3 labelPosition = new Vector3(floor.transform.position.x, floor.transform.position.y - offset, floor.transform.position.z);
        // textObject.transform.position = labelPosition;
        // LogFileWriter.WriteLog($"Building label position: x={labelPosition.x} y={labelPosition.y} z={labelPosition.z}");

        // Attach the label to the building
        textObject.transform.parent = floor.transform;

        // Adjust rotation if needed
        textObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void AddBuildingLabel(GameObject building, Vector3 buildingPosition, string name)
    {
        GameObject textObject = new GameObject("BuildingLabel");
        TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();

        // Ensure a font asset is assigned
        textMeshPro.font = fontAsset;
        // Set the label's text and appearance
        textMeshPro.text = name;
        textMeshPro.fontSize = 8;
        textMeshPro.color = Color.black;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        MeshRenderer meshRenderer = textObject.GetComponent<MeshRenderer>();

        // Set sorting order of the MeshRenderer
        meshRenderer.sortingOrder = 5;  // Change sorting order here

        Vector3 labelPosition = new Vector3(2*buildingPosition.x+offset, buildingPosition.y - offset, 2*buildingPosition.z+offset);
        textObject.transform.position = labelPosition;
        LogFileWriter.WriteLog($"Building label position: x={labelPosition.x} y={labelPosition.y} z={labelPosition.z}");

        // Attach the label to the building
       // textObject.transform.SetParent(building.transform, false);
        //textObject.transform.parent = building.transform;
        //textObject.transform.rotation = Quaternion.LookRotation(textObject.transform.position - Camera.main.transform.position);


        // Adjust rotation if needed
        //textObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        // Scale down if the text appears too large
        //textObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    
    
    /*
    void AddFloorLabel(GameObject floor, string floorName, int floorNumber)
    {
        GameObject label = new GameObject("Label");
        TextMesh textMesh = label.AddComponent<TextMesh>();

        // Set the label's text
        textMesh.text = floorName;

        // Customize the appearance of the text
        textMesh.fontSize = 9; // Set smaller font size
        textMesh.color = Color.black;

        Vector3 labelPosition =  new Vector3(floor.transform.position.x - cubeSize.x / 2, floor.transform.position.y + cubeSize.y / 2 * floorNumber, floor.transform.position.z+ offset);
        LogFileWriter.WriteLog($"Floor label position: x={labelPosition.x} y={labelPosition.y} z={labelPosition.z}");
        // Set the label's position
        label.transform.position = labelPosition;

        // Attach the label as a child of the floor
        label.transform.parent = floor.transform;

        // Rotate the label to Face the Z-axis
        label.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
  
    void AddBuildingLabel(GameObject building, Vector3 buildingPosition, string name)
    {
        GameObject textObject = new GameObject("Text");
        textObject.transform.position = new Vector3(0, 0, 0);

        TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();


        // GameObject label = new GameObject("Label");
        //TextMesh textMesh = label.AddComponent<TextMesh>();

        // Set the label's text
        textMeshPro.text = name;

        // Customize the appearance of the text
        textMeshPro.fontSize = 9;
        textMeshPro.color = Color.black;
        Vector3 labelPosition =  new Vector3(buildingPosition.x, buildingPosition.y - offset, buildingPosition.z);
        LogFileWriter.WriteLog($"Building label position: x={labelPosition.x} y={labelPosition.y} z={labelPosition.z}");
        // Set the label's position
        textObject.transform.position = labelPosition;

        // Attach the label as a child of the floor
        textObject.transform.parent = building.transform;

        // Rotate the label to Face the Z-axis
        textObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }*/
    

}


