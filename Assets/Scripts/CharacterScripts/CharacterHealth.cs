using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    private float curHealth = 3.0f; // player current health
    public float maxHealth = 3.0f; // number of total health containers visible on screen

    public Image[] healthBar;
    public Sprite fullHealthContainer;
    public Sprite emptyHealthContainer;
    public Sprite halfHealthContainer;
    
    public GameObject DeathMessage;
    [SerializeField] private bool isAlive = true;
    [SerializeField] private bool hasHalfHealth;

    private void OnEnable()
    {
        curHealth = maxHealth;
        isAlive = true;
    }

    private void Update()
    {
        if (isAlive)
        { 
            UpdateHealthBar();
        
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

    private void UpdateHealthBar()
    {
        // see if health has a fraction part
        double halfHealth = curHealth % 1.0;
        hasHalfHealth = (halfHealth == 0.5);
        // get the integer part
        int intHealth = (int)curHealth;
        
        // Update each health container
        for (int i = 0; i < healthBar.Length; i++)
        {
            if (i < intHealth)
                healthBar[i].sprite = fullHealthContainer;
            else
                healthBar[i].sprite = emptyHealthContainer;
            
            if (i < maxHealth)
                healthBar[i].enabled = true;
            else
                healthBar[i].enabled = false;
        }
        
        // add the half full container at the end
        if (hasHalfHealth)
            healthBar[intHealth].sprite = halfHealthContainer;
            
    }
    
    
    /// <summary>
    /// Call this from other scripts when the player (of any form?)is hit by a bullet 
    /// </summary>
    public void HitByBullet(float dmg)
    {
        curHealth -= dmg;
    }

    /// <summary>
    /// Call this from other scripts when the player is exposed to an enemy and will be teleported back 
    /// </summary>
    public void IsExposed()
    {
        curHealth -= 1.0f;
    }
}
