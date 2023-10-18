using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{

   private Camera mainCamera;

   private void Start()
   {
      mainCamera = Camera.main;
   }

   private void Update()
   {
      Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit hit))
      {
         transform.position = hit.point;
      }
     
   }
}
