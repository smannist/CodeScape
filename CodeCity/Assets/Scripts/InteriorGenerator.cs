using UnityEngine;
using Assets.Entities;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class InteriorGenerator : MonoBehaviour
{
	//Class description is written to this
    public GameObject descriptionField;
	public GameObject floor;
	
	public GameObject doorObject;
	
	//transform of this object is used as the starting point of the row
	public GameObject[] doorRowStarts;
	
	
	private Function[] functions;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        functions = Globals.floorContents;
		
		descriptionField.GetComponent<TMPro.TMP_Text>().text = Globals.floorDescription;
		floor.GetComponent<Renderer>().material.color = Globals.floorColor;
		
		float maxDoorsPerWall = 8;
		int doorsLeft = functions.Length;
		int doorIndex = 0;
		for (int row = 0;row<4 && doorsLeft > 0;row++){
			Transform rowStart = doorRowStarts[row].transform;
			int doorsOnWall = (int)Mathf.Min(maxDoorsPerWall, doorsLeft);
			createRow(doorsOnWall, doorIndex, rowStart.position, rowStart.rotation, 1.8f * rowStart.right);
			doorsLeft -= doorsOnWall;
			doorIndex += doorsOnWall;
		}
    }
	
	void createRow(int count, int firstFuncIdx, Vector3 startPos, Quaternion rotation, Vector3 direction){
		for(int i = 0;i<count;i++)
		{
			Vector3 pos = startPos + (i * direction);
			GameObject obj = Instantiate(doorObject, pos, rotation);
			obj.transform.parent = transform;
			
			string name = functions[i + firstFuncIdx].name;
            string desc = functions[i + firstFuncIdx].description;
            obj.GetComponentInChildren<TMPro.TextMeshPro>().text = name;

            // Add the DoorHandler script
            DoorHandler roomGenerator = obj.AddComponent<DoorHandler>();
            // Assign the function name to the door
            roomGenerator.functionName = name;
            roomGenerator.functionDesc = desc;
        }
	}


    // Update is called once per frame
    /*void Update()
    {
		//return to city on click
        if (Input.GetMouseButtonDown(0))
        {
			SceneManager.LoadScene("SampleScene");   
		}
    }*/
    void Update()
    {
        // Detect left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Perform a raycast from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the raycast hits a collider
            if (Physics.Raycast(ray, out hit))
            {
                // If the hit object has the DoorHandler component, let it handle the click
                DoorHandler doorHandler = hit.collider.GetComponent<DoorHandler>();
                if (doorHandler != null)
                {
                    // Let the DoorHandler script handle this click
                   // doorHandler.OpenDoor();
                    return; // Exit early to avoid executing the scene change logic
                }
            }

            // If no door was clicked, return to the default scene
            SceneManager.LoadScene("SampleScene");
        }
    }
 
}
