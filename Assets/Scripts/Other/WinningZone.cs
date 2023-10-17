using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class WinningZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            Debug.Log("Player enters the winning zone.");
            EventCenter.GetInstance().TriggerEvent("PlayerWins", this);
        }
    }
}
