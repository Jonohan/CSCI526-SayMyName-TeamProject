using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsWithoutCollider : MonoBehaviour
{
    public GameObject followObject;
    private GameObject child;
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        child.transform.position = new Vector3(followObject.transform.position.x, child.transform.position.y, followObject.transform.position.z);
    }
}
