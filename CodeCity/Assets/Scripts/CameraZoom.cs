using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera;          // Reference to the camera you want to zoom in and out
    public float zoomSpeed = 10f;      // Speed of zoom (how fast the camera zooms)
    public float minZoom = 5f;         // Minimum zoom (closeness to the object)
    public float maxZoom = 50f;        // Maximum zoom (farthest distance)

    private void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Assign the main camera if not already assigned
        }

        // Zoom in and out with the scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            // Adjust camera's field of view (for perspective camera)
            mainCamera.fieldOfView -= scrollInput * zoomSpeed;

            // Clamp the field of view within the min and max limits
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 10f, 60f);
        }
    }
}
