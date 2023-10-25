using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int maxHealth = 3;
    public int restHealth;

    // Start is called before the first frame update
    void Start()
    {
        // initialize the restHealth to the maxHealth
        restHealth = maxHealth;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
