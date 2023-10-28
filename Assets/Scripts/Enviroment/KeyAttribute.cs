using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KeyAttribute : MonoBehaviour
{
    [Header("Key Attribute")]
    public Transform charTransform;
    public Vector3 offset;
    [SerializeField] private bool isPickedUp = true;

    public delegate void KeyDroppedHandler();
    public event KeyDroppedHandler OnKeyDropped; // when key is dropped make a event

    // when key is picked up, it would follow the player or enemy
    // else it can be picked up by player
    private void Update()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.x = 0;
        rotation.y = 0; // Lock the X & Y rotation
        transform.eulerAngles = rotation;

        if (charTransform != null && charTransform.gameObject.activeInHierarchy)
        {
            transform.position = charTransform.position + offset; // Make Key follow the character (above)
        }
        else
        {
            isPickedUp = false;
            OnKeyDropped?.Invoke();
            // event: enemy drop the key
        }


    }

    // Get isPickedUp
    public bool IsKeyPickedUp
    {
        get { return isPickedUp; }
    }


    /*    private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                pickUpKey(other.transform);
            }
        }*/

    /**
    *  else when enemy die, drop the key, and player can pick up the key
    */
    /*    private void pickUpKey(Transform playerTransform)
        {
            charTransform = playerTransform;
            isPickedUp = true;
        }*/
}
