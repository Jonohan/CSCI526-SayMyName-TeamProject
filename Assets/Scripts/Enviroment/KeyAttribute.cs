using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAttribute : MonoBehaviour
{
    public Transform CharTransform;
    public Vector3 offset; 


    private void Update()
    {
         /**
         *  if Key is picked up by any character
         */
        transform.position = CharTransform.position + offset; // Make Key follow the character (above)

        /**
        *  else when enemy die, drop the key, and player can pick up the key
        */
    }
}
