using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    public TMP_Text playerHintText;
    public GameObject attackManager;
    private WaterAttackManager waterAttackManager;
    public GameObject originalPlayer, redHint, greenHint;
    private CharController playerController;

    // Start is called before the first frame update
    void Start()
    {
        waterAttackManager = attackManager.GetComponent<WaterAttackManager>();
        playerController = originalPlayer.GetComponent<CharController>();


    }

    // Update is called once per frame
    void Update()
    {
        int enemyCount = waterAttackManager.patrolEnemy;
        if (enemyCount >= 0 && enemyCount<3)
        {
            playerHintText.text = "Press 2 to trap";
        }
        if (enemyCount >=3 && playerController.currentState == CharController.PlayerState.Normal)
        {
            playerHintText.text = "Press 3 to fighting state";
        }
        if (enemyCount >= 3 && playerController.currentState == CharController.PlayerState.Fighting)
        {
            playerHintText.text = "Right Click to shoot";
        }

        if (GameObject.Find("EnemyGB2") == null)
        {
            redHint.SetActive(false);
        }
        if (GameObject.Find("PatrolEnemy2") == null)
        {
            greenHint.SetActive(false);
        }
    }
}
