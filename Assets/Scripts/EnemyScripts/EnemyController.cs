using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
         CharacterHealth ch = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<CharacterHealth>();
         ch.curHealth -= 1;
      }
   }
}
