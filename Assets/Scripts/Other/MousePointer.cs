using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{

   private Camera mainCamera;
   public LayerMask mouseLayerMask;

   private void Start()
   {
      mainCamera = Camera.main;
      LayerMask mouseIgnoredLayerMask = LayerMask.GetMask("IgnoreMouseRaycast");
      mouseLayerMask = ~mouseIgnoredLayerMask;
   }

   private void Update()
   {
      Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
     
      if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mouseLayerMask))
      {
         transform.position = hit.point;
      }
     
   }
   
   /// <summary>
   /// Ensures that the MousePointerInitialized event is triggered exactly 2 frames after Start(). 
   /// </summary>
   /// <returns></returns>
   private IEnumerator CRT_TriggerMousePterEvent()
   {
      yield return null;
      yield return null;
      EventCenter.GetInstance().TriggerEvent("MousePointerInitialized", this);
   }
}
