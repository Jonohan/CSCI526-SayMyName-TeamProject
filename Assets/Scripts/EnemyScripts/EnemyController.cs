using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   public float viewDistance;
   public float viewAngle;
   public Vector3 initialPosition;
   private float lastDetectionTime = -5.0f; 
   private float detectionCooldown = 1.0f; // 设定一个1秒的冷却时间

   private void Update()
   {
      DetectPlayer();
   }
   
   private void Start()
   {
      initialPosition = transform.position;
   }
   
   void DetectPlayer()
   {
      if (Time.time - lastDetectionTime < detectionCooldown) 
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
               break;
            case CharController.PlayerState.Fighting:
               playerHealth.curHealth -= Mathf.FloorToInt(0.5f);
               break;
         }
      }
   }
   
   
   public void ReturnToPosition()
   {
      UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
      if (agent)
      {
         agent.SetDestination(initialPosition);
      }
   }
}
