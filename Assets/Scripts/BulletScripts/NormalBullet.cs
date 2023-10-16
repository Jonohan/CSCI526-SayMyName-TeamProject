using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    [Header("General Bullet Properties")]
    public float lifeCycle = 5.0f;   // Total lifespan of the bullet before it gets recycled
    public float damageAmount = 1.0f;  
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isBulletReady = false;

    [Header("Properties Assigned by Shooter")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector3 moveDir;  // Direction in which the bullet should move

    private void OnEnable()
    {
        // Retrieve the Rigidbody component and set a method to recycle the bullet after its life cycle
        rb = this.GetComponent<Rigidbody>();
        Invoke("RecycleObj", lifeCycle);
    }

    private void FixedUpdate()
    {
        // If the bullet is not ready, exit the method
        if (!isBulletReady) return;
        
        // Move the bullet in the specified direction at the specified speed
        rb.MovePosition(transform.position + moveDir.normalized * bulletSpeed * Time.deltaTime); 
    }

    // Prepare the bullet for recycling: reset its state and return it to the object pool
    private void RecycleObj()
    {
        isBulletReady = false;
        rb.velocity = Vector3.zero;
        ObjPoolManager.GetInstance().PushObj(this.gameObject.name, this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Check if the bullet collided with the player
        if (other.collider.CompareTag("Player"))
        {
            // Reduce player's health by the bullet's damage amount
            CharacterHealth playerHealth = other.gameObject.GetComponent<CharacterHealth>();
            if (playerHealth)
            {
                playerHealth.curHealth -= damageAmount;
            }
        }
        // Recycle the bullet after the collision
        RecycleObj();
    }

    // Method to set up the bullet's properties when it's taken from the object pool and fired
    public void InitializeNormalBullet(Vector3 targetDir, float speed, float damage)
    {
        this.bulletSpeed = speed;
        this.moveDir = targetDir.normalized;
        this.damageAmount = damage;
        this.rb.velocity = Vector3.zero;
        isBulletReady = true;
    }
}