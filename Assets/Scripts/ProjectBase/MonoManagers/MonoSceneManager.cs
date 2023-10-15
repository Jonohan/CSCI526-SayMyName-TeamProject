using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoSceneManager : MonoBehaviour
{

    public GameObject WinScreen;
    
    private void OnEnable()
    {
        if (WinScreen == null)
        {
            WinScreen = GameObject.Find("WinScreen");
        }
        EventCenter.GetInstance().AddEventListener("PlayerWins", ShowWinScreen);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartGame();

        else if (Input.GetKeyDown(KeyCode.Q))
            Application.Quit();
    }
    
    private void RestartGame()
    {
        // Remove all events from dictionary
        EventCenter.GetInstance().ClearEvents();
        // Clear object pool
        ObjPoolManager.GetInstance().ClearPool();
        Debug.Log("Restarting Scene...\n");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowWinScreen(object info)
    {
        // Disable player from controlling character 
        PossessionManager pm = GameObject.Find("MonoPossessionManager").GetComponent<PossessionManager>();
        pm.originalPlayer.GetComponent<CharController>().enabled = false;
        
        WinScreen.SetActive(true);
    }

    public void MSM_LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        //Debug.Log("Total scenes = " + totalScenes.ToString());
        int nextSceneIndex = currentSceneIndex + 1;
        if (currentSceneIndex >= totalScenes - 1)
            nextSceneIndex = 0;
        Debug.Log("Current Scene: " + currentSceneIndex.ToString());
        Debug.Log("Loading the next level...\n");
        EventCenter.GetInstance().ClearEvents();
        ObjPoolManager.GetInstance().ClearPool();
        SceneManager.LoadScene(nextSceneIndex);
    }
    
}
