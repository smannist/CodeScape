using UnityEngine;
using Assets.Entities;
using UnityEngine.SceneManagement;

public class FunctionFloor : MonoBehaviour
{

    public Function[] functions;
    public string filename;
    public string fileOverview;

    void OnClick(){
        CameraController b = Object.FindFirstObjectByType<CameraController>();
        if(b != null){
            b.ZoomIn(this.gameObject);
        }
    }
	
    void OnDoubleClick()
    {
        // Enter floor
        Globals.floorContents = functions;
        Globals.floorDescription = filename + "\n" + fileOverview;
        Globals.floorColor = this.gameObject.GetComponent<Renderer>().material.color;
        SceneManager.LoadScene("FloorInterior");   
    }
}
