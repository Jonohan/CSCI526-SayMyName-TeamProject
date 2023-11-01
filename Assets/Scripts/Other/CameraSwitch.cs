using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;
    
    public Transform cameraPosition1;
    public Transform cameraPosition2;

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
            MoveCamera(cameraPosition1);
        }
    }

    // Move the camera to the new position and rotation
    private void MoveCamera(Transform newCameraPosition)
    {
        mainCamera.transform.position = newCameraPosition.position;
        mainCamera.transform.rotation = newCameraPosition.rotation;
        Debug.Log("Camera moved to new position and rotation");
    }
    /*    // When player enter the collider box, switch to the corresponding camera
        public Camera camera1;
        public Camera camera2;

        //public GameObject collider1;
        //public GameObject collider2;

        void Start()
        {
            Debug.Log("Camera Switch Script Started");
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                Debug.Log("Trigger Entered: " + other.name);

                    camera1.gameObject.SetActive(true);
                    camera2.gameObject.SetActive(false);
                    Debug.Log("Switching to Camera 1");

            }
        }*/

}
