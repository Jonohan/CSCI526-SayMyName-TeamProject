using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterScript : MonoBehaviour
{

    public GameObject manager = null;
    //public Text enemyKillCountText;

    [Header("Count setting")]
    public int initialPatrolEnemyCount = 0;

    private WaterAttackManager waterAttackManager;

    // Start is called before the first frame update
    void Start()
    {
        waterAttackManager = manager.GetComponent<WaterAttackManager>();

        waterAttackManager.enemy = initialPatrolEnemyCount;
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
            waterAttackManager.enemy += 1;

            collision.gameObject.SetActive(false);
            Debug.Log("Swallow EnemyGB amount:" + waterAttackManager.enemy);
            waterAttackManager.recover();

            //UpdateEnemyKillText(waterAttackManager.enemy, "EnemyGB");
        }

        if (collision.gameObject.name.Contains("PatrolEnemy"))
        {
            waterAttackManager.patrolEnemy += 1;

            collision.gameObject.SetActive(false);
            Debug.Log("Swallow patrolEnemy amount:" + waterAttackManager.patrolEnemy);
            waterAttackManager.recover();

            //UpdateEnemyKillText(waterAttackManager.patrolEnemy, "PatrolEnemy");
        }

        if (waterAttackManager.patrolEnemy >= 3 || waterAttackManager.enemy >= 3)
        {
            waterAttackManager.canEnterFightingState = true;
            Debug.Log("You have collected 3 same type bodies, press 3 to Imitate them.");
        }
    }


/*    // Need to show different type enemy at the same screen in next version
    private void UpdateEnemyKillText(int count, string enemyType)
    {
        if (enemyType == "EnemyGB")
        {
            enemyKillCountText.text = $"Red Enemy Devour Count: {count}/3";
        }
        else if (enemyType == "PatrolEnemy")
        {
            enemyKillCountText.text = $"Green Enemy Devour Count: {count}/3";
        }
    }*/

    // get count
    public int GetEnemyGBCount()
    {
        return waterAttackManager.enemy;
    }

    public int GetPatrolEnemyCount()
    {
        return waterAttackManager.patrolEnemy;
    }


}
/*
private void OnCollisionEnter(Collision collision)
{
    Debug.Log("Collision with " + collision.gameObject.name);

    if (collision.gameObject.name.Contains("EnemyGB"))
    {
        waterAttackManager.enemy += 1;

        collision.gameObject.SetActive(false);
        Debug.Log("Swallow EnemyGB amount:" + waterAttackManager.enemy);
        waterAttackManager.recover();

        UpdateEnemyKillText(waterAttackManager.enemy, "EnemyGB");
    }

    if (collision.gameObject.name.Contains("PatrolEnemy"))
    {
        waterAttackManager.patrolEnemy += 1;

        collision.gameObject.SetActive(false);
        Debug.Log("Swallow patrolEnemy amount:" + waterAttackManager.patrolEnemy);
        waterAttackManager.recover();

        UpdateEnemyKillText(waterAttackManager.patrolEnemy, "PatrolEnemy");
    }

    if (waterAttackManager.patrolEnemy >= 3 || waterAttackManager.enemy >= 3)
    {
        waterAttackManager.canEnterFightingState = true;
        Debug.Log("You have collected 3 same type bodies, press 3 to Imitate them.");
    }
}


// Need to show different type enemy at the same screen in next version
private void UpdateEnemyKillText(int count, string enemyType)
{
    if (enemyType == "EnemyGB")
    {
        enemyKillCountText.text = $"Red Enemy Devour Count: {count}/3";
    }
    else if (enemyType == "PatrolEnemy")
    {
        enemyKillCountText.text = $"Green Enemy Devour Count: {count}/3";
    }
}
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterScript : MonoBehaviour
{

public GameObject manager = null;
public Text enemyKillCountText;

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

        UpdateEnemyKillText(manager.GetComponent<WaterAttackManager>().enemy, "EnemyGB");
    }

    if (collision.gameObject.name.Contains("PatrolEnemy"))
    {
        manager.GetComponent<WaterAttackManager>().patrolEnemy += 1;

        collision.gameObject.SetActive(false);
        Debug.Log("Swallow patrolEnemy amount:" + manager.GetComponent<WaterAttackManager>().patrolEnemy);
        manager.GetComponent<WaterAttackManager>().recover();

        UpdateEnemyKillText(manager.GetComponent<WaterAttackManager>().patrolEnemy, "PatrolEnemy");
    }
    //// Check if the collision involves a specific tag
    Debug.Log("Collision with " + collision.gameObject.name);

    if (manager.GetComponent<WaterAttackManager>().patrolEnemy >= 3 || manager.GetComponent<WaterAttackManager>().patrolEnemy >= 3)
    {
        manager.GetComponent<WaterAttackManager>().canEnterFightingState = true;
        Debug.Log("You have collected 3 same type bodies, press 3 to Imitate them.");
    }
}

// Need to show different type enemy at the same screen in next version
private void UpdateEnemyKillText(int count, string enemyType)
{
    if (enemyType == "EnemyGB")
    {
        enemyKillCountText.text = $"Red Enemy Devour Count: {count}/3";
    }
    else if (enemyType == "PatrolEnemy")
    {
        enemyKillCountText.text = $"Green Enemy Devour Count: {count}/3";
    }
}
}*/