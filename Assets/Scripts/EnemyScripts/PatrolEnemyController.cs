using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PatrolEnemyController : MonoBehaviour
{
    public float viewDistance = 5;
    public float viewAngle = 90;
    public Vector3 initialPosition;
    private float lastDetectionTime = -5.0f;
    private float detectionCooldown = 1.0f; // 设定一个1秒的冷却时间

    private bool findPlayer = false;
    GameObject HP_Bar;
    Image HP_Image;
    float HP;
    float HP_Percent;
    private float Max_HP = 10;

    public Transform[] points; // Patrolling Turning points
    private int destPoint = 0;
    private int patrolDirction = 1;

    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; // ban speeding down when approaching the destination

        GotoNextPoint();

        // initiate the health bar for the enemy
        HP_Bar = GameObject.Find("HBar");
        HP_Image = HP_Bar.GetComponent<Image>();
        HP = Max_HP;

    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();

        // When agent is approaching the current destination point,
        // go to the next point
        if (!agent.pathPending && agent.remainingDistance < 1.5f)
            GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // No points or only one point
        if (points.Length == 0 || points.Length == 1)
            return;

        // the length of points must be >= 2
        agent.destination = points[destPoint].position;
        destPoint = destPoint + patrolDirction;

        // change the patrolling direction => reverse
        if (destPoint == points.Length - 1)
        {
            patrolDirction = -1;
        } else if(destPoint == 0)
        {
            patrolDirction = 1;
        }
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

    void DetectPlayer()
    {
        if (Time.time - lastDetectionTime < detectionCooldown) // during the cooldown time, do nothing
            return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            CharController player = playerObject.GetComponent<CharController>();
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            if (angle < viewAngle / 2.0f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dirToPlayer, out hit, viewDistance))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        lastDetectionTime = Time.time; // 更新最后检测时间
                        OnPlayerSpotted(player);
                    }
                }
            }
        }
    }
    void OnPlayerSpotted(CharController player)
    {
        CharacterHealth playerHealth = player.GetComponent<CharacterHealth>();

        if (player && playerHealth)
        {
            switch (player.currentState)  // Assuming you've defined currentState in CharController
            {
                case CharController.PlayerState.Normal:
                    playerHealth.curHealth -= 1;
                    player.TeleportToStart();  // Assuming you've defined TeleportToStart in CharController
                    break;
                case CharController.PlayerState.Possessing:
                    // TO DO
                    break;
                case CharController.PlayerState.Fighting:
                    playerHealth.curHealth -= Mathf.FloorToInt(0.5f);
                    agent.Stop();
                    break;
            }
        }
    }
    public void ReturnToPosition()
    {
        if (agent)
        {
            agent.SetDestination(initialPosition);
        }
    }
}
