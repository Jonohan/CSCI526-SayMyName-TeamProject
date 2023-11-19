using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharController : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] private Camera mainCamera;

    [Header("Character General Properties")]
    [SerializeField] private Rigidbody rb;
    //[SerializeField] private bool isPuddle = false;

    [Header("Character Movement Parameters")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 720;

    [Header("Skills Parameters - Possession Bullet")]
    public float PossessionSkillCooldown = 1.0f;
    private float lastSkillUseTime = -5.0f;
    public float bulletSpeed = 20f;

    [Header("Skills Parameters - Damage Bullet")]
    public float fireInterval = 1.5f;
    private float lastShotTime = -1.5f;
    public float dBulletSpeed = 20f;

    [Header("User Input")]
    [SerializeField] private Vector3 input = Vector3.zero;
    
    [Header("LogText")]
    public GameObject LogTextContainer;
    private Text logText;
    private Text debugText;

    [Header("Debug: Mouse Aiming")]
    [SerializeField] private bool isAiming = false;
    [SerializeField]private GameObject mousePointer;
    [SerializeField] private Vector3 mouseRealWorldPos;
    [SerializeField] private Quaternion rot;
    private LayerMask lm;
    
    [Header("Inventory System")]
    public GameObject myBag;
    bool isOpen;

    [Header("Particle Systems")]
    public GameObject psAimingSelfContainer;
    public ParticleSystem psAimingSelf;
    private Coroutine FXCoroutine = null;
    public bool isCoroutineRunning = false;
    

    // for data collection
    private int possessionBulletCount = 0;
    private int damageBulletCount = 0;
    
    // UI
    public GameObject stateTextContainer;
    private Text stateText;

    public enum PlayerState
    {
        Normal,
        Possessing,
        Fighting
    }

    public PlayerState currentState;
    public Vector3 startPosition;

    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            myBag.SetActive(isOpen);
        }
    }

    private void OnEnable()
    { 
        // ensures that this event is listened BEFORE mouse pointer is actually initialized
        EventCenter.GetInstance().AddEventListener("MousePointerInitialized", GetMousePointer);
        // Listen to an event where possession is officially done and returned to the player's original body
        EventCenter.GetInstance().AddEventListener("ReturnedFromPossession", PlayReturnFx);
    }

    private void Start()
    {
        Debug.Log("CharController Start()");
        input = Vector3.zero;
        rb = this.GetComponent<Rigidbody>();
        logText = LogTextContainer.GetComponent<Text>();
        logText.text = "Possession bullet is ready";
        if (stateTextContainer != null)
        {
            stateText = stateTextContainer.GetComponent<Text>();
            stateText.text = "Stealth";
        }

        //Get main camera
        mainCamera = Camera.main;

        //currentState = PlayerState.Fighting; // The initial status is set to Fighting (force to one status)
        startPosition = transform.position; // Set the starting position
        
        // Initial fx 
        if (psAimingSelfContainer == null)
            psAimingSelfContainer = GameObject.Find("AimFX");
        if (psAimingSelf == null)
            psAimingSelf = psAimingSelfContainer.GetComponent<ParticleSystem>();
        
        StartCoroutine(PlayParticleEffect(psAimingSelf, 0.7f));
    }

    void Update()
    {
        if (stateTextContainer != null)
        {
            // Update status of player
            if (currentState == PlayerState.Normal)
            {
                stateText.text = "Stealth";
            }
            else if (currentState == PlayerState.Fighting)
            {
                stateText.text = "Fighting";
            }
        }

        //Debug.Log(currentState);
        GatherMovingInput();
        OpenMyBag();
        //if (InteractWithUI()) return;
        //MouseControl();

        if (currentState == PlayerState.Normal)
        {
            // Check if the skill is off cooldown
            if (Time.time - lastSkillUseTime >= PossessionSkillCooldown)
            {
                logText.text = "Right Click to shoot a possession bullet.";
            }
        }
        else if (currentState == PlayerState.Fighting)
        {
            // Check if the skill is off cooldown
            if (Time.time - lastShotTime >= fireInterval)
            {
                // need to classify the attack modes in next version
                logText.text = "Right Click to start killing :)";
            }
            else // not ready yet
            {
                logText.text = "Reloading";
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            //if possession is off cooldown and the coroutine is not running
            if (Time.time - lastSkillUseTime >= PossessionSkillCooldown && !isCoroutineRunning)
            {
                FXCoroutine = StartCoroutine(PlayParticleEffect(psAimingSelf, 0.7f));
            }
            // if possession is off cooldown but the coroutine is already running
            else if (Time.time - lastSkillUseTime >= PossessionSkillCooldown && isCoroutineRunning)
            {
                // If we have this running coroutine's reference 
                if (FXCoroutine != null)
                {
                    // Stop the current coroutine and start a new one
                    StopCoroutine(FXCoroutine);
                    FXCoroutine = StartCoroutine(PlayParticleEffect(psAimingSelf, 0.7f));
                }
            }
            // FXCoroutine = StartCoroutine(PlayParticleEffect(psAimingSelf, 1.5f));
        }
        
        Look();

        if (Input.GetMouseButtonUp(1))
        {
            // Check if the skill is off cooldown
            if (currentState == PlayerState.Normal)
            {
                if (Time.time - lastSkillUseTime >= PossessionSkillCooldown)
                {
                    ShootPossessionBullet(); // only in normal status
                    logText.text = "";
                    possessionBulletCount++;
                    //Debug.Log("P:" + possessionBulletCount);
                }
            }
            else if (currentState == PlayerState.Fighting)
            {
                if (Time.time - lastShotTime >= fireInterval)
                {
                    ShootDamageBullet();// only in fighting status
                    logText.text = "Reloading";
                    damageBulletCount++;
                    //Debug.Log("F:" + damageBulletCount);
                }
            }
            isAiming = false;

            psAimingSelf.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!isAiming)
            Move();
        
        // update fx position
        if (isCoroutineRunning)
            psAimingSelfContainer.transform.position = gameObject.transform.position;
    }

    /// <summary>
    /// Gathers keyboard (WASD) input from the user. Called once per frame.
    /// </summary>
    void GatherMovingInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")); // comparing to GetAxis this one does not have a smoothing in or out, resulting in no 'inertia'.
    }
    
    
    /// <summary>
    /// Rotate the player game object, according to either 1) keyboard input, or 2) aiming mouse input
    /// </summary>
    void Look()
    {
        Vector3 relativeDiff = Vector3.zero;

        if (isAiming)
        {
            relativeDiff = mousePointer.transform.position - transform.position;
            relativeDiff.y = 0;
            var rotation = Quaternion.LookRotation(relativeDiff);
            transform.rotation = rotation;
        }

        else if (input != Vector3.zero)
        {
            // Rotate input because of isometric view (see helper IsoHelper.cs).
            Vector3 isoInput = input.ToIsoView();

            // relative difference between char's current position and the direction where it wants to go 
            relativeDiff = ((transform.position + isoInput) - transform.position);
            var rotation = Quaternion.LookRotation(relativeDiff, Vector3.up);
            transform.rotation = rotation;
        }
    }

    /// <summary>
    /// Move the player game object towards the direction it wants to go. Called once per FixedUpdate() ONLY IF !isAiming. 
    /// </summary>
    void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * input.magnitude) * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Spawn a PossessionBullet prefab from object pool; assign shooter, velocity and speed to it.
    /// The bullet's movement is handled in PossessionBullet's script.
    /// In order for the prefab to spawn correctly it needs to be put into Resources/Prefabs.
    /// </summary>
    private void ShootPossessionBullet()
    {
        // Manually change this path for now. 
        GameObject bullet = ObjPoolManager.GetInstance().GetObj("Prefabs/PossessionBullet");
        // Call initialization method in PossessionBullet script. It will handle everything from here.
        PossessionBullet pb = bullet.GetComponent<PossessionBullet>();
        pb.InitializePossessionBullet(this.gameObject, transform.forward, this.bulletSpeed);
        // Reset skill CD.
        lastSkillUseTime = Time.time;
    }

    // When player is in fighting status, it can shoot bullet to damage bullet
    // In the version, bullets just simply destory the enemy object (need health bar in next version)
    private void ShootDamageBullet()
    {
        GameObject bullet = ObjPoolManager.GetInstance().GetObj("Prefabs/DamageBullet");
        CharDamageBullet db = bullet.GetComponent<CharDamageBullet>();
        db.InitializeDamageBullet(this.gameObject, transform.forward, this.bulletSpeed);
        // Reset skill CD.
        lastShotTime = Time.time;
    }
    
    public void TeleportToStart()
    {
        transform.position = startPosition; // Reset player position to start
    }

    private IEnumerator PlayParticleEffect(ParticleSystem ps, float duration)
    {
        isCoroutineRunning = true;
        psAimingSelfContainer.transform.position = gameObject.transform.position;
        ps.gameObject.SetActive(true);
        var main = ps.main;
        main.startColor = GetComponent<Renderer>().material.color;
        ps.Play();
        yield return new WaitForSeconds(duration);
        ps.gameObject.SetActive(false);
        isCoroutineRunning = false;
    }


    // Get bullet count
    public int GetPossessionBulletCount()
    {
        return possessionBulletCount;
    }

    public int GetDamageBulletCount()
    {
        return damageBulletCount;
    }

    /// <summary>
    /// At the beginning of game, when MousePointer game object is initialized, this function will be called when MousePointer
    /// triggers this event. This makes sure CharController has the reference of the mouse pointer. 
    /// </summary>
    /// <param name="info"> Passed in by event center. A MousePointer class component.</param>
    private void GetMousePointer(object info)
    {
        MousePointer mp = info as MousePointer;
        if (mp != null)
        {
            mousePointer = mp.gameObject;
            lm = mp.mouseLayerMask;
        }
    }

    /// <summary>
    /// Start the coroutine of playing the possession effect. 
    /// </summary>
    /// <param name="info"> Passed in by the event center. CharController object. Irrelevant to this function.</param>
    private void PlayReturnFx(object info)
    {
        // if currently no FX coroutine is running
        if (FXCoroutine == null)
        {
            FXCoroutine = StartCoroutine(PlayParticleEffect(psAimingSelf, 0.7f));
        }
        else
        {
            // Stop the current coroutine and start a new one
            StopCoroutine(FXCoroutine);
            FXCoroutine = StartCoroutine(PlayParticleEffect(psAimingSelf, 0.7f));
        }
    }
}
