using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenus : MonoBehaviour
{

    public void BackToMenu()
    {

        EventCenter.GetInstance().ClearEvents();
        ObjPoolManager.GetInstance().ClearPool();


        SceneManager.LoadScene("Content");
        Debug.Log("back to content");
    }
}
