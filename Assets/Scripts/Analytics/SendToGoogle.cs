using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL;
    //This is for test 
    //private string URL2 = "https://docs.google.com/forms/d/e/1FAIpQLSee8Exk_8V1VbOrZ4bOxtao76L8DFinIWB0Qr106AC9uJ1bsQ/viewform?embedded=true";

    private long _sessionID;
    private string currentSceneName;
    private int _patrolEnemy;
    private int _normalEnemy;

    private bool toSend = true;

    public GameObject manager = null;

    private void Awake()
    {
        _sessionID = DateTime.Now.Ticks;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    // Also send if player win
    private void OnEnable()
    {
        EventCenter.GetInstance().AddEventListener("PlayerWins", PlayerWinsHandler);
    }
    private void PlayerWinsHandler(object eventData)
    {
        Send();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<CharacterHealth>().isAlive == false && toSend == true)
        {
            Debug.Log("Sending Metrics...");
            Send();
            toSend = false;
        }
        if(this.GetComponent<CharacterHealth>().isAlive == true)
        {
            toSend = true;
        }
    }

    public void Send()
    {
        _patrolEnemy = manager.GetComponent<WaterAttackManager>().patrolEnemy;
        _normalEnemy = manager.GetComponent<WaterAttackManager>().enemy;
        StartCoroutine(Post(_sessionID.ToString(), _patrolEnemy.ToString(), _normalEnemy.ToString()));
        //StartCoroutine(Post2(_sessionID.ToString(), _patrolEnemy.ToString(), _normalEnemy.ToString()));
    }

    private IEnumerator Post(string sessionID, string patrolEnemy, string normalEnemy)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.427865542", sessionID);
        form.AddField("entry.981299839", currentSceneName);
        form.AddField("entry.165316135", patrolEnemy);
        form.AddField("entry.816835344", normalEnemy);

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if(www.result !=  UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

/*    private IEnumerator Post2(string sessionID, string patrolEnemy, string normalEnemy)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1240078272", sessionID);
        form.AddField("entry.374845860", currentSceneName);
        form.AddField("entry.1990401853", patrolEnemy);
        form.AddField("entry.1664227036", normalEnemy);

        using (UnityWebRequest www = UnityWebRequest.Post(URL2, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete2!");
            }
        }
    }*/
}