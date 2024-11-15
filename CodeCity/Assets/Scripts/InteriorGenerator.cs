using UnityEngine;
using Assets.Entities;
using UnityEngine.SceneManagement;


public class InteriorGenerator : MonoBehaviour
{
	//Class description is written to this
    public GameObject descriptionField;
	
	public GameObject doorObject;
	
	//transform of this object is used as the starting point of the row
	public GameObject doorRowStart;
	
	private Class floorClass;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        floorClass = Globals.enteredFloor;
		
		Debug.Log("Running interior generator: " + floorClass.name);
		descriptionField.GetComponent<TMPro.TMP_Text>().text = floorClass.name + "\n" + floorClass.description;
		
		createRow();
    }
	
	void createRow(){
		Debug.Log("Creating " + floorClass.functions.Length + " doors");
		for(int i = 0;i<floorClass.functions.Length;i++)
		{
			createDoor(i);
			
		}
	}
	
	void createDoor(int index){
		string name = floorClass.functions[index].name;
		Vector3 doorSpacing = new Vector3(doorObject.transform.localScale[0] * 4, 0, 0);
		Debug.Log(index + " is at " + doorRowStart.transform.position + index*doorSpacing);
		GameObject obj = Instantiate(doorObject, doorRowStart.transform.position + index*doorSpacing, doorRowStart.transform.rotation);
		obj.GetComponentInChildren<TMPro.TextMeshPro>().text = name;
		
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
