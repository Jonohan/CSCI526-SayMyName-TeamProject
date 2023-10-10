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
    [Header("Count")]
    [SerializeField] private int waterLeft = 4;


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

        // Player turn into water when player pressed 3 and is meeting all conditions
        if (originalPlayer.activeInHierarchy && waterLeft > 0 && (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)))
        {

            originalPlayer.SetActive(false);
            water.SetActive(true);
            lastTimeSkillUsed = Time.time;
            waterLeft -= 1;
            Debug.Log("Player turned into water!");
        }

        // Turn back to original player after 10s
        if (water.activeInHierarchy && (Time.time - lastTimeSkillUsed > 10.0f))
        {
            water.SetActive(false);
            originalPlayer.SetActive(true);
            Debug.Log("Player back from water!");
        }

    }
}