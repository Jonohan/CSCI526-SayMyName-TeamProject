using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.XR;

public class PlayerControllerPossessed : MonoBehaviour
{
    [Header("Character General Properties")]
    [SerializeField] private Rigidbody rb;
    
    [Header("Character Movement Parameters")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 720;
    
    [Header("User Input")]
    [SerializeField] private Vector3 input = Vector3.zero;
    
    [Header("LogText (need to move to UIManager later)")]
    [SerializeField] private GameObject LogTextContainer;
    private Text logText;

    private void OnEnable()
    {
        // once we can confirm that possession has started, trigger this event for background color change and stuff
        Debug.Log("PlayerControllerPossessed: Possession started. ");
        EventCenter.GetInstance().TriggerEvent("PossessionStartsSuccessfully", this);
    }

    private void Start()
    {
        input = Vector3.zero;
        rb = this.GetComponent<Rigidbody>();
        // TODO: Move this to UIManager later.
        LogTextContainer = GameObject.Find("LogTextContainer");
        logText = LogTextContainer.GetComponent<Text>();
        logText.text = "Possession bullet and Puddle are disabled in this form. Press 1 to return to your body.";
        
    }

    // Update is called once per frame
    void Update()
    {
        GatherMovingInput();
        Look();
        
        // If user presses '1' then trigger the event of returning to original body
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            EventCenter.GetInstance().TriggerEvent("ReturnToOgBody", this);
            
            // Moved this part to PossessionManager -NW
            // possessedEnemy.ReturnToPosition();  // ÐÂÔö
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
}
