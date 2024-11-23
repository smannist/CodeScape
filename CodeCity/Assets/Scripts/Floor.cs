using UnityEngine;
using Assets.Entities;
using UnityEngine.SceneManagement;
using TMPro;

public class Floor : MonoBehaviour
{
    
    public Class classObj;
    
	void Start(){
        //set label to mach classObj
        TextMeshPro[] textMeshPro = this.gameObject.GetComponentsInChildren<TextMeshPro>();
        for(int i = 0;i<textMeshPro.Length;i++)
        {
        textMeshPro[i].text = classObj.name;
        }
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
        Globals.floorContents = classObj.methods;
        Globals.floorDescription = classObj.name + "\n" + classObj.description;
        Globals.floorColor = this.gameObject.GetComponent<Renderer>().material.color;
        SceneManager.LoadScene("FloorInterior");   
    }

}
