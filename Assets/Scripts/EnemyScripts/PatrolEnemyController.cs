using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatrolEnemyController : MonoBehaviour
{
    private bool findPlayer = false;
    GameObject HP_Bar;
    Image HP_Image;
    float HP;
    float HP_Percent;
    private float Max_HP = 10;

    // Start is called before the first frame update
    void Start()
    {
        // initiate the health bar for the enemy
        HP_Bar = GameObject.Find("HBar");
        HP_Image = HP_Bar.GetComponent<Image>();
        HP = Max_HP;

    }

    // Update is called once per frame
    void Update()
    {
        if (findPlayer == false)
        {
            Patrol();
        } else
        {

        }
    }

    // If the patrol enemy does not see the player, then keep patroling
    void Patrol()
    {

    }

    // The player enter the patrolling area
    void OnTriggerEnter(Collider collider)
    {
        if(HP > 0)
        {
            HP -= 2;
            HP_Percent = HP / Max_HP;
            HP_Image.fillAmount = HP_Percent;
            Debug.Log(HP_Percent);
        }
        
        
        /*if (collider.gameObject.tag == "Player")
        {
            // Shoot at the player
        } else if(collider.gameObject.tag == "") // Hit by a assalt bullet, rather than possession bullet
        {
            // decrease the health bar
            HP -= 1;
            HP_Percent = HP / Max_HP;
            HP_Image.fillAmount = HP_Percent;
            Debug.Log(HP_Percent);
        }*/
    }

    // The player exits the patrolling area
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            // Stop shooting at the player

        }
    }
}
