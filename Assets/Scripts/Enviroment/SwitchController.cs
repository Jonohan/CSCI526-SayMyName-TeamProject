using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public DoorController doorController; // Call Door controller method
    private bool isPlayerInRange = false; 

    void Update()
    {
        if (isPlayerInRange)
        {
            /**
             * Need to implement the notice "Press E to open the door"
             */
            if(Input.GetKeyDown(KeyCode.E))
            {
                doorController.OpenDoor(); // if character is in collide range and press E, open the door
            }
        }
    }


    // Get in and get out the trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")|| other.CompareTag("Enemy")) 
        {
            isPlayerInRange = true;
            Debug.Log("Press E to open the door.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy")) 
        {
            isPlayerInRange = false; 
        }
    }



}
