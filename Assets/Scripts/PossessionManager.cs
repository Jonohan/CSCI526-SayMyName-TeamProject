using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    
    public GameObject originalPlayer = null;
    [SerializeField] private GameObject currentPlayerControllable = null;
    
    
    void Start()
    {
        Debug.Log("Possession Manager Start()");
        EventCenter.GetInstance().AddEventListener("PossessionSequence", InitiatePossession);
        EventCenter.GetInstance().AddEventListener("ReturnToOgBody", ReturnToOriginalBody);
        currentPlayerControllable = originalPlayer;
    }


    private void InitiatePossession(object info)
    {
        List<GameObject> objList = info as List<GameObject>;
        // if shooter and the possessed are all valid game objects
        if (objList != null && objList.Count == 2)
        {
            Debug.Log(objList[0].gameObject.name + " shoots a possession bullet onto "+ objList[1].gameObject.name);
            
            // Temporarily disable the original character's controller and attach a new controller to the possessed
            currentPlayerControllable.GetComponent<CharController>().enabled = false;
            
            // get the possessed enemy (temporary player)
            GameObject tempPlayer = objList[1];
            
            // disable the enemy controller on it
            // TODO: Replace EnemyController with any actual script name 
            tempPlayer.GetComponent<EnemyController>().enabled = false;
            
            // attach a possessed player controller script
            tempPlayer.AddComponent<PlayerControllerPossessed>();
            RegisterCurrentPlayerControllable(tempPlayer.gameObject);
            
            Debug.Log("Possession started. ");
        }
    }

    /// <summary>
    /// Set the currentPlayerControllable field in the PossessionManager instance 
    /// </summary>
    /// <param name="player">Current controllable 'player' object. Could be a possessed enemy. </param>
    public void RegisterCurrentPlayerControllable(GameObject player)
    {
        this.currentPlayerControllable = player;
        Debug.Log("Possession Manager: Current player registered. Player game object name: " + player.gameObject.name);
    }
    

    private void ReturnToOriginalBody(object info)
    {
        Debug.Log("ReturnToOriginalBody() started. Current time: "+ Time.time.ToString());
        
        // reactivate the original body's CharController script
        originalPlayer.GetComponent<CharController>().enabled = true;
        
        // destroy the possession controller on the enemy
        PlayerControllerPossessed pcp = currentPlayerControllable.GetComponent<PlayerControllerPossessed>();
        if (pcp != null)
            Destroy(pcp);
        
        // reactivate the enemy script
        EnemyController ec = currentPlayerControllable.GetComponent<EnemyController>();
        if (ec != null)
            ec.enabled = true;
        else
            throw new Exception("Error: original enemy controller not found on possessed enemy.");

        StartCoroutine(WaitEnemyReturnToPosition(ec));
        //EventCenter.GetInstance().TriggerEvent("PossessionEnded", this);
        
        this.currentPlayerControllable = originalPlayer;
    }

    /// <summary>
    /// This coroutine ensures that enemy script is enabled before ReturnToPosition() is called,
    /// to avoid possible null reference exception.
    /// </summary>
    /// <param name="ec">EnemyController component.</param>
    /// <returns></returns>
    private IEnumerator WaitEnemyReturnToPosition(EnemyController ec)
    {
        yield return null;
        ec.ReturnToPosition();
    }
}
