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

    public GameObject waterAttackManager = null;
    public GameObject charCon = null;
    public GameObject possManager = null;
    public GameObject monoSceneManager = null;

    private float firstPossTime;
    private float gameOverTime;

    private float timeFromStartToPossess = 0;
    private float timeFromPossessToWin = 0;

    private int bulletKilledEnemies = 0;
    private int waterKilledEnemies = 0;
    private int totalKilledEnemies = 0;

    private int totalPossessedCount = 0;

    private bool hasGetFirstPossTime = false;

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
        EventCenter.GetInstance().AddEventListener("KilledOneEnemy", BulletKillOneEnemy);
        EventCenter.GetInstance().AddEventListener("PossessionSequence", GetFirstPossessTime);
    }

    private void GetFirstPossessTime(object arg0)
    {
        if(hasGetFirstPossTime == false)
        {
            firstPossTime = Time.time;
            hasGetFirstPossTime = true;
            Debug.Log("firstPossTime: " + firstPossTime);
        }
    }

    private void PlayerWinsHandler(object eventData)
    {
        _ifWin = 1;
        gameOverTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        /*        _possessionBulletCount = charCon.GetComponent<CharController>().GetPossessionBulletCount();
                _damageBulletCount = charCon.GetComponent<CharController>().GetDamageBulletCount();

                Debug.Log("P=" + _possessionBulletCount);
                Debug.Log("D=" + _damageBulletCount);*/

        if ((this.GetComponent<CharacterHealth>().isAlive == false || _ifWin == 1) && toSend == true)
        {
            if(this.GetComponent<CharacterHealth>().isAlive == false)
            {
                _ifLose = 1;
            }
            
            Debug.Log("Sending Metrics...");
            Send();
            toSend = false;
        }
    }

    private void BulletKillOneEnemy(object info)
    {
        bulletKilledEnemies++;
    }

    public void Send()
    {
        _patrolEnemy = waterAttackManager.GetComponent<WaterAttackManager>().patrolEnemy;
        _normalEnemy = waterAttackManager.GetComponent<WaterAttackManager>().enemy;
        _possessionBulletCount = charCon.GetComponent<CharController>().GetPossessionBulletCount();
        _damageBulletCount = charCon.GetComponent<CharController>().GetDamageBulletCount();
        _puddleCount = waterAttackManager.GetComponent<WaterAttackManager>().countWater;

        //StartCoroutine(Post(_sessionID.ToString(), _patrolEnemy.ToString(), _normalEnemy.ToString(), _possessionBulletCount.ToString(), _damageBulletCount.ToString(), _ifWin.ToString(), _ifLose.ToString()));

        //Debug.Log("Possession Bullet Count: " + _possessionBulletCount);
        //Debug.Log("Damage Bullet Count: " + _damageBulletCount);
        //Debug.Log("Puddle Count: " + _puddleCount);

        string startToPossess = "";
        string possessToEnd = "";

        if (_ifWin == 1)
        {
            startToPossess = firstPossTime.ToString();
            possessToEnd = (gameOverTime - firstPossTime).ToString();
        }

        waterKilledEnemies = waterAttackManager.GetComponent<WaterAttackManager>().GetTotalEnemyCount();
        totalKilledEnemies = waterKilledEnemies + bulletKilledEnemies;
        Debug.Log("totalKilledEnemies:" + totalKilledEnemies);

        totalPossessedCount = possManager.GetComponent<PossessionManager>().GetPossessedCount();
        Debug.Log("totalPossessedCount:" + totalPossessedCount);

        StartCoroutine(Post(
            _possessionBulletCount.ToString(),
            _damageBulletCount.ToString(), _puddleCount.ToString(), 
             startToPossess, possessToEnd));
    }

    private IEnumerator Post(string possessionBulletCount, string damageBulletCount, 
        string puddleCount, string timeFromStartToPossess, string timeFromPossessToEnd
        )
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.427865542", _sessionID.ToString());
        form.AddField("entry.981299839", currentSceneName);
        form.AddField("entry.165316135", _patrolEnemy);
        form.AddField("entry.816835344", _normalEnemy);

        form.AddField("entry.412904116", possessionBulletCount);
        form.AddField("entry.1821634003", damageBulletCount);
        form.AddField("entry.2138783510", puddleCount);
        form.AddField("entry.1057004087", _ifWin.ToString());
        form.AddField("entry.1988072853", _ifLose.ToString());

        form.AddField("entry.738480559", timeFromStartToPossess);
        form.AddField("entry.725822769", timeFromPossessToEnd);


        form.AddField("entry.1356695298", totalPossessedCount);
        form.AddField("entry.320010246", totalKilledEnemies);
        
        //Debug.Log("Print: "  + patrolEnemy + normalEnemy + possessionBulletCount + damageBulletCount + ifWin + ifLose);

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
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