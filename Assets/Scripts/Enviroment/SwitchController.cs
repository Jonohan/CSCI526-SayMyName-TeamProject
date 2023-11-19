using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchController : MonoBehaviour
{
    public DoorController doorController; // Call Door controller method
    public bool isPlayerInRange = false;
    public bool isDoorOpen = false;
    private CharController charController;
    /*    public GameObject LogTextContainer;
        private Text logText;*/


    /*    private void Start()
        {
            logText = LogTextContainer.GetComponent<Text>();
        }*/

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        charController = playerObject.gameObject.GetComponent<CharController>();
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            /**
             * Need to implement the notice "Press E to open the door"
             */
            if (Input.GetKeyDown(KeyCode.E))
            {
                doorController.OpenDoor(); // if character is in collide range and press E, open the door
                isDoorOpen = true;
            }
        }
    }


    // Get in and get out the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // Should work if Character currentstate can be set to "Possessing" correctly
        // if (other.CompareTag("Player")|| (other.CompareTag("Enemy") && charController.currentState == CharController.PlayerState.Possessing)) 
        if (other.CompareTag("Player")|| other.CompareTag("Enemy")) 
        {
            isPlayerInRange = true;
            Debug.Log("Press E to open the door.");
            //logText.text = "Press E to open the door.";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("Player")|| (other.CompareTag("Enemy") && charController.currentState == CharController.PlayerState.Possessing)) 
        if (other.CompareTag("Player") || (other.CompareTag("Enemy") && charController.currentState == CharController.PlayerState.Possessing)) 
        {
            isPlayerInRange = false;
            //logText.text = ""; // Hide the text when character leave the switch area
        }
    }



}
