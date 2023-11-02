using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillEnemyCount : MonoBehaviour
{
    public WaterScript waterScript; // 引用 WaterScript
    public Slider redEnemySlider;  // 红色敌人的进度条
    public Slider greenEnemySlider; // 绿色敌人的进度条
    public Text redEnemyText; // 红色敌人的提示文字
    public Text greenEnemyText; // 绿色敌人的提示文字

    private void Start()
    {
        if(waterScript != null)
        {
            Debug.Log("WaterScript is successfully referenced!");
        }
        else
        {
            Debug.Log("WaterScript is not referenced!");
        }
        // 初始化进度条和提示文字的状态
        redEnemySlider.value = 0;
        greenEnemySlider.value = 0;
        redEnemyText.gameObject.SetActive(false);
        greenEnemyText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(waterScript != null)
        {
            int redCount = waterScript.GetEnemyGBCount();
            int greenCount = waterScript.GetPatrolEnemyCount();
            redEnemySlider.value = redCount / 3.0f;
            greenEnemySlider.value = greenCount / 3.0f;
            if (redCount >= 3)
            {
                redEnemyText.gameObject.SetActive(true);
            }
            if (greenCount >= 3)
            {
                greenEnemyText.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("WaterScript is not referenced in KillEnemyCount!");
        }
    }
}
