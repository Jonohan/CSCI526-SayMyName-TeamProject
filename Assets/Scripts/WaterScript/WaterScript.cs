using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    int patrolEnemy = 0;
    int enemy = 0;
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
            enemy += 1;
            collision.gameObject.SetActive(false);
            Debug.Log("Swallow EnemyGB amount:" + enemy);
        }

        if (collision.gameObject.name.Contains("PatrolEnemy"))
        {
            patrolEnemy += 1;
            collision.gameObject.SetActive(false);
            Debug.Log("Swallow patrolEnemy amount:" + patrolEnemy);
        }
        //// Check if the collision involves a specific tag
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    // Do something when the collision with a GameObject with the "Player" tag occurs
        //}
        Debug.Log("Collision with " + collision.gameObject.name);
    }
}
