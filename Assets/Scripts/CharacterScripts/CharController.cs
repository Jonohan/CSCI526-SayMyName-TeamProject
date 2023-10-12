using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{   
    [Header("Main Camera")]
    [SerializeField] private Camera mainCamera;
    
    [Header("Character General Properties")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isPuddle = false;
    
    [Header("Character Movement Parameters")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 720;
   
    [Header("Skills Parameters - Possession Bullet")]
    public float PossessionSkillCooldown = 5.0f;
    private float lastSkillUseTime = -5.0f;
    public float bulletSpeed = 20f;
    
    [Header("User Input")]
    [SerializeField] private Vector3 input = Vector3.zero;

    [Header("LogText (need to move to UIManager later?)")]
    public GameObject LogTextContainer;
    private Text logText;

    [Header("Debug: Mouse Aiming")]
    [SerializeField]private Vector3 mouseRealWorldPos;
    [SerializeField]private Vector3 mouseIsoWorldPos;
    [SerializeField]private bool isAiming = false;

    public GameObject myBag;
    bool isOpen;

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
    
    
    
    private void Start()
    {
        Debug.Log("CharController Start()");
        input = Vector3.zero;
        rb = this.GetComponent<Rigidbody>();
        logText = LogTextContainer.GetComponent<Text>();
        logText.text = "Possession bullet is ready";
        
        //Get main camera
        mainCamera = Camera.main;

        currentState = PlayerState.Normal; // ³õÊ¼×´Ì¬ÉèÎªNormal
        startPosition = transform.position; // Set the starting position
    }

    void Update()
    {
        GatherMovingInput();
        OpenMyBag();

        // Check if the skill is off cooldown
        if (Time.time - lastSkillUseTime >= PossessionSkillCooldown)
        {
            logText.text = "Press RMB to shoot possession bullet.";
        }
        else // not ready yet
        {
            logText.text = "Possession bullet is not ready yet.";
        }

        if (Input.GetMouseButtonDown(1))
            isAiming = true;
        
        if (isAiming)
            GatherMouseLookingInput();
        
        Look();
        
        if (Input.GetMouseButtonUp(1))
        { 
            // Check if the skill is off cooldown
            if (Time.time - lastSkillUseTime >= PossessionSkillCooldown)
            {
                ShootPossessionBullet();
                Debug.Log("Possession bullet is not ready");
                logText.text = "Possession bullet is not ready yet.";
            }

            isAiming = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GatherMovingInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")); // comparing to GetAxis this one does not have a smoothing in or out, resulting in no 'inertia'.
    }

    
    void GatherMouseLookingInput()
    {
        mouseRealWorldPos = ScreenToWorld(Input.mousePosition);
        mouseIsoWorldPos = mouseRealWorldPos.IsoToWorld();
    }
    
    Vector3 ScreenToWorld(Vector3 screenPos)
    {
        // Convert screen position to a ray
        Ray ray = mainCamera.ScreenPointToRay(screenPos);

        // Cast ray to game plane (however you define it, e.g., z = 0)
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        // Handle cases where the raycast might not hit anything.
        return Vector3.zero;
    }
    
    void Look()
    {
        Vector3 relativeDiff = Vector3.zero;
        
        if (isAiming)
        {
            // will disregard keyboard input vector
            relativeDiff = mouseIsoWorldPos - transform.position;
            relativeDiff.y = 0;
            var rotation = Quaternion.LookRotation(relativeDiff, Vector3.up);
            transform.rotation = rotation;
        }
        
        else if (input != Vector3.zero)
        {
            // Rotate input because of isometric view (see helper IsoHelper.cs).
            Vector3 isoInput = input.ToIsoView();
            
            // relative difference between char's current position and the direction where it wants to go 
            relativeDiff = ( (transform.position + isoInput) - transform.position);
            var rotation = Quaternion.LookRotation(relativeDiff, Vector3.up);
            transform.rotation = rotation;
        }
    }

    void Move()
    {
        rb.MovePosition(transform.position + ( transform.forward * input.magnitude ) * moveSpeed * Time.deltaTime);
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
        pb.InitializePossessionBullet(this.gameObject, this.transform.forward, this.bulletSpeed);
        // Reset skill CD.
        lastSkillUseTime = Time.time;
    }

    private void DebugFunc(object info)
    {
        Debug.Log("this function is invoked by triggering event DebugE");
    }

    public void TeleportToStart()
    {
        transform.position = startPosition; // Reset player position to start
    }
}