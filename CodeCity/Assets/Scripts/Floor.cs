using UnityEngine;
using Assets.Entities;
using UnityEngine.SceneManagement;
using TMPro;

public class Floor : MonoBehaviour
{
    
    public Class classObj;
    
	void Start(){
		//set label to mach classObj
		TextMeshPro textMeshPro = this.gameObject.GetComponentInChildren<TextMeshPro>();
		textMeshPro.text = classObj.name;
	}
	
    void OnClick(){
        CameraController b = Object.FindFirstObjectByType<CameraController>();
        if(b != null){
            b.ZoomIn(this.gameObject);
        }
    }
    
    void OnDoubleClick()
    {
        // Enter floor
        Globals.enteredFloor = classObj;
        Globals.floorColor = this.gameObject.GetComponent<Renderer>().material.color;
        SceneManager.LoadScene("FloorInterior");   
    }

}
