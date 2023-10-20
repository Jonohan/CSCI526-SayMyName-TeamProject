using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [Header("Character Health")]
    private float curHealth = 3.0f; // player current health
    public float maxHealth = 3.0f; // number of total health containers visible on screen
    [SerializeField] public bool isAlive = true;
    [SerializeField] private bool hasHalfHealth;

    [Header("Health Bar UI Specifics")]
    public Image[] healthBar;
    public Sprite fullHealthContainer;
    public Sprite emptyHealthContainer;
    public Sprite halfHealthContainer;
    
    [Header("Player Death Message")]
    public GameObject DeathMessage;
    
    [Header("Change Sprite Color With Possession")]
    [SerializeField] private BackgroundColorChange bcc;
    [SerializeField] private Color intendedHealthBarColor;
    // this remains unchanged throughout the game, unaffected by players current color.
    [SerializeField] private Color currentHealthBarColor = new Color(0.65f, 0.08f, 0.73f);
    private bool needInitializeColor = true;
    private Camera mainCam;
    
    private void OnEnable()
    {
        curHealth = maxHealth;
        isAlive = true;
        mainCam = Camera.main;
        bcc = mainCam.GetComponent<BackgroundColorChange>();
        EventCenter.GetInstance().AddEventListener("PossessionStartsSuccessfully", DelayChangeHealthColor);
        EventCenter.GetInstance().AddEventListener("ReturnToOgBody", DelayChangeHealthColorBack);
        
        // Change the health container's color to the original player's material color.
        intendedHealthBarColor = GetComponent<Renderer>().material.color;
        
        // Change the sprite color to player's color first 
        ChangeHealthColor(null);
        needInitializeColor = false;
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

    /// <summary>
    /// Change health container sprite color to the enemy's color when player is possessing an enemy
    /// </summary>
    /// <param name="info">PlayerControllerPossessed component passed by event center. Not relevant in this function.</param>
    private void ChangeHealthColor(object info)
    {
        // In onEnable call, leave the intended color unchanged.
        // During runtime, change them dynamically
        if (!needInitializeColor)
        {
            // Get the possessed enemy's color from BackgroundColorChange.cs
            // This is the color we want the health bar to be.
            intendedHealthBarColor = bcc.GetCurrentObjColor();
        }
       
        // Change color of the images by setting inputs for the material
        for (int i = 0; i < healthBar.Length; i++)
        {
            Image image = healthBar[i];
            Material imageMat = image.material;
            
            imageMat.SetColor("_IntendedColor", intendedHealthBarColor);
            imageMat.SetColor("_CurrentColor", currentHealthBarColor);
        }
    }

    /// <summary>
    /// Change health container color back to the original player's color after possession ends.
    /// </summary>
    /// <param name="info">Possession pair passed to event center. Not relevant in this function.</param>
    private void ChangeHealthColorBack(object info)
    {
     
        intendedHealthBarColor =  gameObject.GetComponent<Renderer>().material.color;
     
        // Change color of the images by setting inputs for the material
        for (int i = 0; i < healthBar.Length; i++)
        {
            Image image = healthBar[i];
            Material imageMat = image.material;
            
            imageMat.SetColor("_IntendedColor", intendedHealthBarColor);
            imageMat.SetColor("_CurrentColor", currentHealthBarColor);
        }
    }
    
    /// <summary>
    /// Called when event "PossessionSequence" is triggered. Will start a coroutine CO_ChangeHealthColor.
    /// //TODO: better implementation?
    /// </summary>
    /// <param name="info">Possession pair passed to event center. Not relevant in this function.</param>
    private void DelayChangeHealthColor(object info)
    {
        StartCoroutine(CO_ChangeHealthColor());
    }
    
    /// <summary>
    /// Called when event "ReturnToOgBody" is triggered. Will start a coroutine CO_ChangeHealthColorBack.
    /// //TODO: better implementation?
    /// </summary>
    /// <param name="info">Possession pair passed to event center. Not relevant in this function.</param>
    private void DelayChangeHealthColorBack(object info)
    {
        StartCoroutine(CO_ChangeHealthColorBack());
    }
    
    private IEnumerator CO_ChangeHealthColor()
    {
        yield return null;
        ChangeHealthColor(null);
    }
    
    private IEnumerator CO_ChangeHealthColorBack()
    {
        yield return null;
        ChangeHealthColorBack(null);
    }

}
