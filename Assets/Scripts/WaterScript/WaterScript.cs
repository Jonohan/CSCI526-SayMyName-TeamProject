using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{

    public GameObject manager = null;
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
        Debug.Log("Collision with " + collision.gameObject.name);
        if (collision.gameObject.name.Contains("EnemyGB"))
        {
            manager.GetComponent<WaterAttackManager>().enemy += 1;

            collision.gameObject.SetActive(false);
            Debug.Log("Swallow EnemyGB amount:" + manager.GetComponent<WaterAttackManager>().enemy);
            manager.GetComponent<WaterAttackManager>().recover();
        }

        if (collision.gameObject.name.Contains("PatrolEnemy"))
        {
            manager.GetComponent<WaterAttackManager>().patrolEnemy += 1;

            collision.gameObject.SetActive(false);
            Debug.Log("Swallow patrolEnemy amount:" + manager.GetComponent<WaterAttackManager>().patrolEnemy);
            manager.GetComponent<WaterAttackManager>().recover();
        }
        //// Check if the collision involves a specific tag
        Debug.Log("Collision with " + collision.gameObject.name);
    }
}
