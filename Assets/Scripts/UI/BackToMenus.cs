using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenus : MonoBehaviour
{

    public void BackToMenu()
    {
        SceneManager.LoadScene("Content");
        Debug.Log("back to content");
    }
}
