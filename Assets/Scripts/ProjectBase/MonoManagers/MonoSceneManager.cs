using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoSceneManager : MonoBehaviour
{
    public GameObject Tutorial;
    public GameObject WinScreen;
    public GameObject NextLevelButton;
    private GameObject mousePointer;
    private float winTime = 0;
    private void OnEnable()
    {
        Tutorial.SetActive(false);
        if (WinScreen == null)
        {
            WinScreen = GameObject.Find("WinScreen");
        }
        EventCenter.GetInstance().AddEventListener("PlayerWins", ShowWinScreen);
        mousePointer = GameObject.Find("MousePointer");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartGame();

        // else if (Input.GetKeyDown(KeyCode.Q))
        //     Application.Quit();
    }
    
    private void RestartGame()
    {
        // Remove all events from dictionary
        EventCenter.GetInstance().ClearEvents();
        Debug.Log("Event center clear.");
        // Clear object pool
        ObjPoolManager.GetInstance().ClearPool();
        Debug.Log("Object pool clear.");
        Debug.Log("Restarting Scene...\n");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowWinScreen(object info)
    {
        // Disable player from controlling character 
        PossessionManager pm = GameObject.Find("MonoPossessionManager").GetComponent<PossessionManager>();
        pm.originalPlayer.GetComponent<CharController>().enabled = false;

/*        // Send the data to google form
        SendToGoogle sendToGoogle = GetComponent<SendToGoogle>();
        if (sendToGoogle != null)
        {
            sendToGoogle.Send();
        }
        else
        {
            Debug.LogError("SendToGoogle component not found on the GameObject.");
        }*/

        WinScreen.SetActive(true);

        if (NextLevelButton != null)
        {
            NextLevelButton.SetActive(true);    
        }

        winTime = Time.time;
    }

    public void MSM_LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        //Debug.Log("Total scenes = " + totalScenes.ToString());
        int nextSceneIndex = currentSceneIndex + 1;
        if (currentSceneIndex >= totalScenes - 1)
            nextSceneIndex = 0;

        if (currentSceneIndex == 1)
        {
            WinScreen.SetActive(false);
        }
        Debug.Log("Current Scene: " + currentSceneIndex.ToString());
        Debug.Log("Loading the next level...\n");
        EventCenter.GetInstance().ClearEvents();
        ObjPoolManager.GetInstance().ClearPool();
        SceneManager.LoadScene(nextSceneIndex);
    }

    public float GetWinTime()
    {
        return winTime;
    }
    
}
