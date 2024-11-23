using UnityEngine;
using Assets.Entities;
using UnityEngine.SceneManagement;


public class InteriorGenerator : MonoBehaviour
{
	//Class description is written to this
    public GameObject descriptionField;
	public GameObject floor;
	
	public GameObject doorObject;
	
	//transform of this object is used as the starting point of the row
	public GameObject[] doorRowStarts;
	
	private Class floorClass;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        floorClass = Globals.enteredFloor;
		
		//Set text on sign
		descriptionField.GetComponent<TMPro.TMP_Text>().text = floorClass.name + "\n" + floorClass.description;
		floor.GetComponent<Renderer>().material.color = Globals.floorColor;
		
		float maxDoorsPerWall = 8;
		int doorsLeft = floorClass.methods.Length;
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
			
			string name = floorClass.methods[i + firstFuncIdx].name;
			obj.GetComponentInChildren<TMPro.TextMeshPro>().text = name;
		}
	}


    // Update is called once per frame
    void Update()
    {
		//return to city on click
        if (Input.GetMouseButtonDown(0))
        {
			SceneManager.LoadScene("SampleScene");   
		}
    }
}
