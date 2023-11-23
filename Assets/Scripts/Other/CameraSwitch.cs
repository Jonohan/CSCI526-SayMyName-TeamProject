using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;
    
    public Transform cameraPosition1;
    public Transform cameraPosition2;

    private float transitionDuration = 1.0f;

    void Start()
    {
        // Set the initial camera position and rotation
        mainCamera.transform.position = cameraPosition1.position;
        mainCamera.transform.rotation = cameraPosition1.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveCameraSmoothly(cameraPosition1));
        }
    }

    // Move the camera to the new position and rotation

    private IEnumerator MoveCameraSmoothly(Transform newCameraPosition)
    {
        float elapsed = 0;

        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        while (elapsed < transitionDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, newCameraPosition.position, elapsed / transitionDuration);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, newCameraPosition.rotation, elapsed / transitionDuration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = newCameraPosition.position;
        mainCamera.transform.rotation = newCameraPosition.rotation;

        Debug.Log("Camera moved to new position and rotation smoothly");
    }

}
