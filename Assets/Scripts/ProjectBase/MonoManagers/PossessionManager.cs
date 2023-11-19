using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    
    public GameObject originalPlayer = null;
    [SerializeField] private GameObject currentPlayerControllable = null;
    [SerializeField] private ParticleSystem psPoss;
    public GameObject FxContainer;

    private int possessedCount = 0;

    void Start()
    {
        Debug.Log("Possession Manager Start()");
        EventCenter.GetInstance().AddEventListener("PossessionSequence", InitiatePossession);
        EventCenter.GetInstance().AddEventListener("ReturnToOgBody", ReturnToOriginalBody);
        EventCenter.GetInstance().AddEventListener("PossessionStartsSuccessfully", PlayPossessionEffect);
        currentPlayerControllable = originalPlayer;
    }

    private void InitiatePossession(object info)
    {
        List<GameObject> objList = info as List<GameObject>;
        // if shooter and the possessed are all valid game objects
        //if (objList != null && objList[0] != null && objList[1] != null)
        if (objList != null && objList.Count == 2)
        {
            Debug.Log(objList[0].gameObject.name + " shoots a possession bullet onto "+ objList[1].gameObject.name);
            
            // get the possessed enemy (temporary player)
            GameObject tempPlayer = objList[1];
            
            // disable the enemy controller on it
            tempPlayer.GetComponent<EnemyController>().enabled = false;
            
            // attach a possessed player controller script
            tempPlayer.AddComponent<PlayerControllerPossessed>();
            PlayerControllerPossessed pcc = tempPlayer.GetComponent<PlayerControllerPossessed>();
            // Assign the VFX object
            pcc.AssignFXContainer(FxContainer);
            
            // Temporarily disable the original character's controller and attach a new controller to the possessed
            CharController cc = objList[0].GetComponent<CharController>();
            cc.enabled = false;
            //currentPlayerControllable.GetComponent<CharController>().enabled = false;
            
            RegisterCurrentPlayerControllable(tempPlayer.gameObject);

            possessedCount++;
            
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
        Debug.Log("Possession Manager: ReturnToOgBody Event triggered.");
        
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
        
        this.currentPlayerControllable = originalPlayer;

        CharController cc = originalPlayer.GetComponent<CharController>();
        // Trigger the event to play the possession fx
        EventCenter.GetInstance().TriggerEvent("ReturnedFromPossession", cc);
        
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

    /// <summary>
    /// Play the particle effect
    /// </summary>
    /// <param name="info">PlayerControllerPossessed component</param>
    private void PlayPossessionEffect(object info)
    {
        Debug.Log("Possession Manager: PlayPossessionEffect is triggered.");
        psPoss = FxContainer.GetComponent<ParticleSystem>();
        if (psPoss != null)
        {
            psPoss.gameObject.SetActive(false);
            GameObject Enemy = (info as PlayerControllerPossessed).gameObject;
            Vector3 newPos = Enemy.transform.position;
            FxContainer.transform.position = newPos;
            var main = psPoss.main;
            main.startColor = Enemy.GetComponent<Renderer>().material.color;

            StartCoroutine(PlayParticleEffect(psPoss, 0.7f));
        }
    }
    
    private IEnumerator PlayParticleEffect(ParticleSystem ps, float duration)
    {
        ps.gameObject.SetActive(true);
        ps.Play();
        yield return new WaitForSeconds(duration);
        ps.gameObject.SetActive(false);
    }

    public int GetPossessedCount()
    {
        return possessedCount;
    }
}
