using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorHandler : MonoBehaviour
{
    internal string functionName;
    internal string functionDesc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }

    // Update is called once per frame
  /*  void Update()
    {
        //return to city on click
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("room click");
            SceneManager.LoadScene("FloorInterior");
            GameObject textObject = GameObject.Find("RoomDescription");

            if (textObject != null)
            {
                // Get TextMeshPro component
                TextMeshPro textMeshPro = textObject.GetComponent<TextMeshPro>();

                if (textMeshPro != null)
                {
                    textMeshPro.text = functionDesc;
                }
            }
            else
            {

            }
        }
    }*/

    private void OnMouseDown()
    {
        // Log the door clicked
        Debug.Log($"Door clicked: {functionName}");

        // Add your custom logic here
        OpenDoor();
    }

    private void OpenDoor()
    {
        Globals.enteredRoom = new Assets.Entities.Function 
                                { name = functionName,
                                  description = functionDesc};
        Debug.Log($"Opening the door: {functionName}");
        SceneManager.LoadScene("RoomInterior");
        // Add logic to switch scenes, display rooms, etc.
    }
}
