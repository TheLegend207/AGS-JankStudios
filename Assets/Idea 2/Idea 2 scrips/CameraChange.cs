using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public Camera playerCamera;
    public Camera mapView;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera.enabled = true;
        mapView.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerCamera.enabled = !playerCamera.enabled;
            mapView.enabled = !mapView.enabled;
        }
    }
}
