using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoSceneManager : MonoBehaviour
{

    public GameObject WinScreen;
    
    private void OnEnable()
    {
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
        Debug.Log("Restarting Scene...\n");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowWinScreen(object info)
    {
        Debug.Log("Show win screen.");
    }
    
}
