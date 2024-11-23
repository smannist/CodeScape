using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInterior : MonoBehaviour
{
    public GameObject FunctionNameField;
    public GameObject descriptionField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //descriptionField.GetComponent<TMPro.TMP_Text>().text = "test Ovini";
        if (descriptionField == null && Globals.enteredRoom != null)
        {
            Debug.LogError("descriptionField is not assigned in the Inspector!");
        }
        else
        {
            FunctionNameField.GetComponent<TMPro.TMP_Text>().text = Globals.enteredRoom.name;
            descriptionField.GetComponent<TMPro.TMP_Text>().text = Globals.enteredRoom.description;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //return to city on click
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("FloorInterior");   
        }
    }
}
