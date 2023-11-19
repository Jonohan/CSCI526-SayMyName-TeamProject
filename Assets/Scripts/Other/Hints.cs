using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Hints : MonoBehaviour
{
    public GameObject followObject;
    private GameObject child;
    private float originalY;
    private bool isKeyHint = false;
    private bool isSwitchHint = false;
    private SwitchController sc;

    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;
        transform.GetChild(0).gameObject.transform.Translate(0, -100, 0);

        if (followObject.CompareTag("Key"))
            isKeyHint = true;

        else if (followObject.CompareTag("Switch"))
        {
            sc = followObject.GetComponent<SwitchController>();
            if (sc != null)
                isSwitchHint = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followObject != null)
            child.transform.position = new Vector3(followObject.transform.position.x, child.transform.position.y, followObject.transform.position.z);
        else if (followObject == null && isKeyHint)
            DeactivateSelf();

        if (isSwitchHint && sc.isDoorOpen)
            DeactivateSelf();
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

    private void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }
}
