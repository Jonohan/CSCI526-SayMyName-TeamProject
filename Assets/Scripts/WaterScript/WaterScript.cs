using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Checking collision with other game object
    private void OnCollisionEnter(Collision collision)
    {
        //// Check if the collision involves a specific tag
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    // Do something when the collision with a GameObject with the "Player" tag occurs
        //}
        Debug.Log("Collision with " + collision.gameObject.name);
    }
}
