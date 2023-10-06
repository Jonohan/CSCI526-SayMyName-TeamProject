using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public int curHealth; // player current health
    public int maxHealth; // number of total health containers visible on screen

    public Image[] healthBar;
    public Sprite fullHealthContainer;
    public Sprite emptyHealthContainer;

    public GameObject DeathMessage;
    [SerializeField] private bool isAlive = true;
    private void Update()
    {
        if (isAlive)
        {
            for (int i = 0; i < healthBar.Length; i++)
            {

                if (i < curHealth)
                    healthBar[i].sprite = fullHealthContainer;
                else
                    healthBar[i].sprite = emptyHealthContainer;
            
                if (i < maxHealth)
                    healthBar[i].enabled = true;
                else
                    healthBar[i].enabled = false;
            }
        
            if (curHealth <= 0)
                CharacterDeath();
        }
    }

    private void CharacterDeath()
    {
        // disable character from moving and all other actions
        CharController cc = GetComponent<CharController>();
        cc.enabled = false;
        
        // Show death message
        DeathMessage.SetActive(true);
        // Prevent further checks in Update().
        isAlive = false;
    }
}
