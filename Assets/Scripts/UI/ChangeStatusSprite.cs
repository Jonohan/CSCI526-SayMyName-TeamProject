using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStatusSprite : MonoBehaviour
{
    public Sprite buttonSprite;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener("StartFighting", ChangeSprite);
        EventCenter.GetInstance().AddEventListener("BackToNormalMode", ChangeSprite);
    }

    /// <summary>
    /// Change the status sprite to match player's state. 
    /// </summary>
    /// <param name="info">string "Normal" or "Fighting".</param>
    private void ChangeSprite(object info)
    {
        
        if ((info as String) == "Normal")
        { 
            buttonSprite = Resources.Load<Sprite>("Sprites/Status_normal");
            GetComponent<Image>().sprite = buttonSprite;
        }
        
        if ((info as String) == "Fighting")
        {
            buttonSprite = Resources.Load<Sprite>("Sprites/Status_attack");
            GetComponent<Image>().sprite = buttonSprite;
        }    
    }
    
}
