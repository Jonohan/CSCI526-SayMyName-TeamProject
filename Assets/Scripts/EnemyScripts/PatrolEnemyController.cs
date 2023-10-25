using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PatrolEnemyController : MonoBehaviour
{
    public Transform[] points; // Patrolling Turning points
    private int destPoint = 0;
    private int patrolDirction = 1;

    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; // ban speeding down when approaching the destination

        GotoNextPoint();

    }

    // Update is called once per frame
    void Update()
    {
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
}
