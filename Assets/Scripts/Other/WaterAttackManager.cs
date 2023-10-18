using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttackManager : MonoBehaviour
{
    [Header("Game Object Access")]
    [SerializeField] private GameObject originalPlayer = null;
    [SerializeField] private GameObject water = null;
    //public bool waterStatus = false;
    private float lastTimeSkillUsed = -10.0f;
    [Header("Counting")]
    [SerializeField] private int waterLeft = 5;
    public int enemy = 0;
    public int patrolEnemy = 0;
    public bool canEnterFightingState = false;

    // Start is called before the first frame update
    void Start()
    {
        water.SetActive(false);
        Debug.Log("WaterAttackManager Start()");

    }

    // Update is called once per frame
    void Update()
    {
        // Update water's location
        water.transform.position = new Vector3(originalPlayer.transform.position.x, water.transform.position.y, originalPlayer.transform.position.z);

        // Player turn into water when player pressed 2 and is meeting all conditions
        if (originalPlayer.activeInHierarchy && waterLeft > 0 && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)))
        {
            //originalPlayer.SetActive(false)
            // move player to y=-20 to hide player
            originalPlayer.transform.position = new Vector3(originalPlayer.transform.position.x, -20f, originalPlayer.transform.position.z);
            //disable player's controler
            originalPlayer.GetComponent<CharController>().enabled = false;
            water.SetActive(true);
            lastTimeSkillUsed = Time.time;
            waterLeft -= 1;
            Debug.Log("Player turned into water!");
        }

        // Turn back to original player after 10s
        if (water.activeInHierarchy && (Time.time - lastTimeSkillUsed > 10.0f))
        {
            water.SetActive(false);
            //originalPlayer.SetActive(true);
            // move player back to original position
            originalPlayer.transform.position = new Vector3(originalPlayer.transform.position.x, 0.5f, originalPlayer.transform.position.z);
            //enable player's controler
            originalPlayer.GetComponent<CharController>().enabled = true;
            Debug.Log("Player back from water!");
        }

        // turn player into fighting state
        if (canEnterFightingState && (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)))
        {
            if (originalPlayer.GetComponent<CharController>().currentState == CharController.PlayerState.Normal)
            {
                originalPlayer.GetComponent<CharController>().currentState = CharController.PlayerState.Fighting;
                Debug.Log("Player is on Fighting state now!");
            }
            else if (originalPlayer.GetComponent<CharController>().currentState == CharController.PlayerState.Fighting)
            {
                originalPlayer.GetComponent<CharController>().currentState = CharController.PlayerState.Normal;
                Debug.Log("Player back to Normal state now!");
            }

        }

    }

    public void recover()
    {
        if (water.activeInHierarchy)
        {
            water.SetActive(false);
            //originalPlayer.SetActive(true);
            // move player back to original position
            originalPlayer.transform.position = new Vector3(originalPlayer.transform.position.x, 0.5f, originalPlayer.transform.position.z);
            //enable player's controler
            originalPlayer.GetComponent<CharController>().enabled = true;
        }
    }

    
}