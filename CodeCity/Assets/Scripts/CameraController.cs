using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{

    // Time threshold for double-click detection (in seconds)
    public float doubleClickThreshold = 0.3f;
    public bool allowCameraControl = false;
    public float turnSpeed = 4.0f;

    // Store the time of the last click
    private float lastClickTime = 0f;
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private Camera mainCamera;
    
    //Horizontal camera rotation
    private float yaw = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         if (mainCamera == null)
         {
         	mainCamera = Camera.main;
         	initialCameraPosition = mainCamera.transform.position;
         	initialCameraRotation = mainCamera.transform.rotation;
         }
    }


    // Update is called once per frame
    void Update()
    {
        
            if (allowCameraControl)
            {
                handleMouseMovements();
            }
            handleClicks();
    }
    
    void handleMouseMovements(){
        yaw += turnSpeed * Input.GetAxis("Mouse X");
        mainCamera.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
    }

	void handleClicks(){
        if (SceneManager.GetActiveScene().name == "SampleScene")
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
                    // Get object that was hit by ray
                    GameObject hitObj = hit.collider.gameObject;

                    float currentTime = Time.time;
                    if (currentTime - lastClickTime <= doubleClickThreshold)
                    {
                        Debug.Log("Double-click detected on: " + hitObj.name + " (tag:" + hitObj.tag + ")");
                        hitObj.SendMessage("OnDoubleClick", null, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        Debug.Log("Single-click detected on: " + hitObj.name + " (tag:" + hitObj.tag + ")");
                        hitObj.SendMessage("OnClick", null, SendMessageOptions.DontRequireReceiver);
                    }

                    // Update the last click time
                    lastClickTime = currentTime;
                }
                else
                {
                    Debug.Log("No object was hit by the ray.");
                    ZoomOut();
                }
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
            mainCamera.transform.rotation = initialCameraRotation;
            Debug.Log("Camera reset to default view");
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned!");
        }
    }


    public void ZoomIn(GameObject gameObject)
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
}
