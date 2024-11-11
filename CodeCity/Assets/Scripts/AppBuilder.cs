using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

public class AppBuilder : MonoBehaviour
{
    public Camera mainCamera;
    // Time threshold for double-click detection (in seconds)
    public float doubleClickThreshold = 0.3f;

    // Store the time of the last click
    private float lastClickTime = 0f;
    Vector3 initialCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                initialCameraPosition = mainCamera.transform.position;
            }
            LogFileWriter.CreateLog();
            JsonReader jsonReader = FindFirstObjectByType<JsonReader>();
            jsonReader.StartReadingJson();
            BuildingGenerator buildingGenerator = FindFirstObjectByType<BuildingGenerator>();
            buildingGenerator.StartCityGeneration();
        }
        catch (Exception e)
        {
            LogFileWriter.WriteLog("Error Occures");
            LogFileWriter.WriteLog(e.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Detect mouse clicks (or taps on mobile devices)
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Debug.Log("Mouse clicked in the view at: " + Input.mousePosition);
            // Convert the mouse position to a ray from the camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast and check if it hits any object
            if (Physics.Raycast(ray, out hit))
            {
                // Get the tag of the object that the ray hit
                string hitObjectTag = hit.collider.gameObject.tag;
                // Log the tag of the clicked object
                Debug.Log("Clicked on object with tag: " + hitObjectTag);
                // Now you can check for specific tags, for example:
                if (hitObjectTag == "Floor")
                {
                    Debug.Log("Clicked on an object with tag 'Floor': " + hit.collider.gameObject.name);
                    // Additional logic when the floor is clicked
                    floorClicked(hit.collider.gameObject);
                }
                else
                {
                    Debug.Log("Clicked on object with tag: " + hitObjectTag);
                }
            }
            else
            {
                Debug.Log("No object was hit by the ray.");
                ZoomOut();
            }
        }
    }


    // Method to reset the camera to the initial position and rotation
    public void ZoomOut()
    {
        if (mainCamera != null)
        {
            // Reset the position and rotation
            mainCamera.transform.position = initialCameraPosition;
            Debug.Log("Camera reset to default view");
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned!");
        }
    }
    private void floorClicked(GameObject gameObject)
    {
        // Check if the clicked object is tagged as "Floor"
        if (gameObject.CompareTag("Floor"))
        {
            Debug.Log("Floor clicked: " + gameObject.name);

            // Check for double-click
            float currentTime = Time.time;
            if (currentTime - lastClickTime <= doubleClickThreshold)
            {
                Debug.Log("Double-click detected on: " + gameObject.name);
                HandleDoubleClick();
            }
            else
            {
                Debug.Log("Single-click detected on: " + gameObject.name);
                ZoomIn(gameObject);
            }

            // Update the last click time
            lastClickTime = currentTime;
        }
        else
        {
            Debug.Log("Clicked outside of a floor. No camera movement.");
        }
    }

    private void ZoomIn(GameObject gameObject)
    {
        if (mainCamera != null)
        {
            Vector3 cameraOffset = new Vector3(0, 5, -10);
            // Adjust the camera offset for better viewing distance
            Vector3 targetPosition = gameObject.transform.position + cameraOffset;

            // Move the camera closer but maintain a smooth transition (optional)
            mainCamera.transform.position = targetPosition;

            // Adjust the camera to look at the object directly
            mainCamera.transform.LookAt(gameObject.transform.position);
        }
        else
        {
            Debug.LogWarning("Main camera not assigned!");
        }
    }

    private void HandleDoubleClick()
    {
        Debug.Log("Handling double-click event on floor: " + gameObject.name);
        // You can add additional behavior for double-click here
        // For example, focus the camera on the floor or show more information.
    }

}
