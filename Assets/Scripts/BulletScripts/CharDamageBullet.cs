using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// We set the all player's bullet at same fire interval 
public class CharDamageBullet : MonoBehaviour
{
    [Header("Player's General Bullet Properties")]
    [SerializeField] private float lifeCycle = 0.5f; // then recycle
    [SerializeField] private float bulletSpeed;
    private Rigidbody rb;
    private GameObject shooter;
    private bool isBulletReady = false;
    private Vector3 moveDir;

    private void OnEnable()
    {
        rb = this.GetComponent<Rigidbody>();
        Invoke("RecycleObj", lifeCycle);
    }

    private void FixedUpdate()
    {
        if (!isBulletReady) return;
        rb.MovePosition(transform.position + moveDir * bulletSpeed * Time.deltaTime);
    }

    // Prepare the bullet for recycling: reset its state and return it to the object pool
    void RecycleObj()
    {
        Debug.Log("RecycleObj called for bullet: " + this.gameObject.name);
        shooter = null;
        isBulletReady = false;
        rb.velocity = Vector3.zero;
        ObjPoolManager.GetInstance().PushObj(this.gameObject.name, this.gameObject);
    }

    // If bullet collode with other objects
    // 
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            Debug.Log("Damage bullet hits an enemy");
            Destroy(other.gameObject);
        }
        // Recycle the bullet after the collision
        RecycleObj();
    }

    public void InitializeDamageBullet(GameObject obj, Vector3 targetDir, float speed)
    {
        this.shooter = obj;
        this.bulletSpeed = speed;
        this.moveDir = targetDir;
        this.transform.position = shooter.transform.position + targetDir * 1.5f;
        this.rb.velocity = Vector3.zero;
        isBulletReady = true;
    }
}
