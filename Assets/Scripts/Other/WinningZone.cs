using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            Debug.Log("Player enters the winning zone.");
            EventCenter.GetInstance().TriggerEvent("PlayerWins", this);
            // Unlock next level in menu
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("levelReached", currentLevel + 1);
            Debug.Log("Level " + (currentLevel + 1) + " unlocked!");
        }
    }
}
