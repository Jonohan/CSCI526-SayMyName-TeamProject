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

    private int _possessionBulletCount = 0;
    private int _damageBulletCount = 0;
    private int _puddleCount = 0;
    private int _ifWin = 0;
    private int _ifLose = 0;

    private bool toSend = true;

    public GameObject manager = null;
    public GameObject charCon = null;

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
        _ifWin = 1;
        Send();
    }

    // Update is called once per frame
    void Update()
    {
/*        _possessionBulletCount = charCon.GetComponent<CharController>().GetPossessionBulletCount();
        _damageBulletCount = charCon.GetComponent<CharController>().GetDamageBulletCount();

        Debug.Log("P=" + _possessionBulletCount);
        Debug.Log("D=" + _damageBulletCount);*/

        if (this.GetComponent<CharacterHealth>().isAlive == false && toSend == true)
        {
            _ifLose = 1;
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
        _possessionBulletCount = charCon.GetComponent<CharController>().GetPossessionBulletCount();
        _damageBulletCount = charCon.GetComponent<CharController>().GetDamageBulletCount();
        _puddleCount = manager.GetComponent<WaterAttackManager>().countWater;
        StartCoroutine(Post(_sessionID.ToString(), _patrolEnemy.ToString(), _normalEnemy.ToString(), _possessionBulletCount.ToString(),_damageBulletCount.ToString(),_puddleCount.ToString(), _ifWin.ToString(),_ifLose.ToString()));
        //StartCoroutine(Post(_sessionID.ToString(), _patrolEnemy.ToString(), _normalEnemy.ToString(), _possessionBulletCount.ToString(), _damageBulletCount.ToString(), _ifWin.ToString(), _ifLose.ToString()));

        //Debug.Log("Possession Bullet Count: " + _possessionBulletCount);
        //Debug.Log("Damage Bullet Count: " + _damageBulletCount);
        //Debug.Log("Puddle Count: " + _puddleCount);
    }

    private IEnumerator Post(string sessionID, string patrolEnemy, string normalEnemy, string possessionBulletCount, string damageBulletCount, string puddleCount, string ifWin, string ifLose)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.427865542", sessionID);
        form.AddField("entry.981299839", currentSceneName);
        form.AddField("entry.165316135", patrolEnemy);
        form.AddField("entry.816835344", normalEnemy);

        
        form.AddField("entry.412904116", possessionBulletCount);
        form.AddField("entry.1821634003", damageBulletCount);
        form.AddField("entry.2138783510", puddleCount);
        form.AddField("entry.1057004087", ifWin);
        form.AddField("entry.1988072853", ifLose);
        //Debug.Log("Print: "  + patrolEnemy + normalEnemy + possessionBulletCount + damageBulletCount + ifWin + ifLose);

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

        _ifWin = 0;
        _ifLose = 0;
    }

}