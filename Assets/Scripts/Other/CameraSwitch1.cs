using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwitch1 : MonoBehaviour
{
    public Camera mainCamera;

    public Transform cameraPosition1;
    public Transform cameraPosition2;

    void Start()
    {
        // Set the initial camera position and rotation
        //mainCamera.transform.position = cameraPosition2.position;
        //mainCamera.transform.rotation = cameraPosition2.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MoveCamera(cameraPosition2);
        }
    }

    // Move the camera to the new position and rotation
    private void MoveCamera(Transform newCameraPosition)
    {
        mainCamera.transform.position = newCameraPosition.position;
        mainCamera.transform.rotation = newCameraPosition.rotation;
        Debug.Log("Camera moved to new position and rotation");
    }
}
