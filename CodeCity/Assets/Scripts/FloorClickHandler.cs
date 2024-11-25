using UnityEngine;

public class DoorClickHandler : MonoBehaviour
{
    // Reference to the main camera
    public Camera mainCamera;

    // Offset to position the camera when moving to the first floor
    public Vector3 cameraOffset = new Vector3(0, 2, -5);

    // Time threshold for double-click detection (in seconds)
    public float doubleClickThreshold = 0.3f;

    // Store the time of the last click
    private float lastClickTime = 0f;

    private void Start()
    {
        // If no camera is assigned, use the main camera in the scene
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Tag the object as "Floor" (ensure your floors are tagged appropriately in Unity)
        gameObject.tag = "Floor";
    }

    // Detect when an object is clicked
    private void OnMouseDown()
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
                EnterFloor();
            }

            // Update the last click time
            lastClickTime = currentTime;
        }
        else
        {
            Debug.Log("Clicked outside of a floor. No camera movement.");
        }
    }

    private void EnterFloor()
    {
        Debug.Log("Entering floor: " + gameObject.name);

        // Move the camera to focus on the first floor
        MoveCameraToFloor();
    }

    private void HandleDoubleClick()
    {
        Debug.Log("Handling double-click event on floor: " + gameObject.name);

        // You can add additional behavior for double-click here
        // For example, focus the camera on the floor or show more information.
    }

    private void MoveCameraToFloor()
    {
        if (mainCamera != null)
        {
            // Calculate the target position: use the current floor's position and add the offset
            Vector3 targetPosition = transform.position + cameraOffset;

            // Move the camera to this position
            mainCamera.transform.position = targetPosition;

            // Optionally, adjust the camera rotation to look at the floor
            mainCamera.transform.LookAt(transform.position);
        }
        else
        {
            Debug.LogWarning("Main camera not assigned!");
        }
    }

    

}
