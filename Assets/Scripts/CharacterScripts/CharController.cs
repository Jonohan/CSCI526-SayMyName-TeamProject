using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    [Header("Character General Properties")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int health = 10;
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

    [Header("LogText (need to move to UIManager later)")]
    public GameObject LogTextContainer;
    private Text logText;
    
    private void Start()
    {
        Debug.Log("CharController Start()");
        input = Vector3.zero;
        rb = this.GetComponent<Rigidbody>();
        logText = LogTextContainer.GetComponent<Text>();
        logText.text = "Possession bullet is ready";
        
    }

    void Update()
    {
        GatherMovingInput();
        Look();
        
        if (Input.GetMouseButtonDown(1))
        {
            // // Check if the skill is off cooldown
            // if (Time.time - lastSkillUseTime >= PossessionSkillCooldown)
            // {
            //     ShootPossessionBullet();
            //     Debug.Log("Possession bullet is not ready");
            //     logText.text = "Possession bullet is not ready yet.";
            // }
            ShootPossessionBullet();
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
    
    void Look()
    {
        if (input != Vector3.zero)
        {
            // Rotate input because of isometric view
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            Vector3 skewedInput = matrix.MultiplyPoint3x4(input);
            
            // relative difference between char's current position and the direction where it wants to go 
            Vector3 relativeDiff = (transform.position + skewedInput) - transform.position;
            var rotation = Quaternion.LookRotation(relativeDiff, Vector3.up);

            // Lerp the rotation for a smoother turn
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);
            
            // // harder turn
            // transform.rotation = rotation;
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

}