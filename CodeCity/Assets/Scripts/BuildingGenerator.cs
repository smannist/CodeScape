using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    private JsonReader jsonReader;
    // Number of stories (floors)
    public int numberOfStories = 5;

    // Size of each cube (floor)
    public Vector3 cubeSize = new Vector3(5, 1, 5);

    // Spacing between each floor
    public float spacing = 0.1f;

    public float offset = 0.9f;

    // Start method to generate the building
    void Start()
    {
        jsonReader = FindObjectOfType<JsonReader>();
        jsonReader.Start();
        // Check if the BuildingManager was found
        if (jsonReader != null)
        {
            int floorCount = jsonReader.GetFloorCount() ?? 0;
            // Get the building count from BuildingManager
            int numberOfStories = floorCount;
            Debug.Log("Number of buildings: " + numberOfStories);

            // Now you can use numberOfBuildings to generate buildings or whatever logic you need
            GenerateBuilding(numberOfStories);
        }
        else
        {
            Debug.LogError("BuildingManager not found in the scene.");
        }
        
    }

    // Method to generate the building with given number of stories
    void GenerateBuilding(int stories)
    {
        for (int i = 0; i < stories; i++)
        {
            // Create a new GameObject for each floor
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Set the size of the floor
            floor.transform.localScale = cubeSize;

            // Position the floor appropriately in the Y-axis
            floor.transform.position = new Vector3(0, i * (cubeSize.y + spacing), 0);

            // Optional: Set the parent of the floor to keep things organized
            floor.transform.parent = this.transform;

            // Add a label to the floor
            AddLabel(floor, i );
        }
    }

    void AddLabel(GameObject floor, int floorNumber)
    {
        GameObject label = new GameObject("Label");
        TextMesh textMesh = label.AddComponent<TextMesh>();

        // Set the label's text
        textMesh.text = "Floor " + floorNumber;

        // Customize the appearance of the text
        textMesh.fontSize = 10; // Set smaller font size
        textMesh.color = Color.black;

        // Position the label on the outside wall of the floor
        //label.transform.position = floor.transform.position + new Vector3(cubeSize.x / 2 + 0.1f, 0, 0);
        // Position the label on the outside of the floor, using the front side (positive Z direction)
        // Offset to prevent text clipping into the wall
        Vector3 labelPosition = floor.transform.position + new Vector3(floor.transform.position.x - cubeSize.x/2, cubeSize.y /2 * floorNumber, offset);

        // Set the label's position
        label.transform.position = labelPosition;

        // Attach the label as a child of the floor
        label.transform.parent = floor.transform;

        // Rotate the label to Face the Z-axis
        label.transform.rotation = Quaternion.Euler(0, 0, 0);
    }





[System.Serializable]
public class FileInfo
{
    public string name;
    public string overview;
    public List<FunctionInfo> functions;
    public List<ClassInfo> classes;
}

[System.Serializable]
public class FunctionInfo
{
    public string name;
    public string description;
}

[System.Serializable]
public class ParamInfo
{
    public string name;
    public string description;
}

[System.Serializable]
public class ClassInfo
{
    public string name;
    public string description;
    public List<FunctionInfo> functions;
}

[System.Serializable]
public class RootObject
{
    public List<FileInfo> files;
}


}
