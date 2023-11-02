using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hints : MonoBehaviour
{
    public GameObject followObject;
    private GameObject child;
    private float originalY;
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;
        transform.GetChild(0).gameObject.transform.Translate(0, -100, 0);




    }

    // Update is called once per frame
    void Update()
    {
        child.transform.position = new Vector3(followObject.transform.position.x, child.transform.position.y, followObject.transform.position.z);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player entered");
        if (other.CompareTag("Player"))
        {
            child.transform.Translate(0, 100, 0);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("player exited");
        if (other.CompareTag("Player"))
        {
           child.gameObject.transform.Translate(0, -100, 0);
        }
    }
}
