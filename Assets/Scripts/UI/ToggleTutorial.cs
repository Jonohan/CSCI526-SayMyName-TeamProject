using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowTutorial : MonoBehaviour
{
    public GameObject tutorial;
    public void ShowtheTutorial()
    {
        tutorial.SetActive(true); // Show the tutorial
        Debug.Log("Showing the tutorial...");
    }

    public void ClosetheTutorial()
    {
        // Close the tutorial
        tutorial.SetActive(false);
        Debug.Log("Closing the tutorial...");
    }
}
