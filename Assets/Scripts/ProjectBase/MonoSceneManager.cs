using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoSceneManager : MonoBehaviour
{
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

}
