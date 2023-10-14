using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PossessionBullet : MonoBehaviour
{     
    // Attach this script to bullets
    [Header("General Bullet Properties")]
    public float lifeCycle = 10.0f; // will be recycled in 5 seconds after instantiated
    public float atk = 0.0f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isBulletReady = false;
    
    [Header("Possession Properties")]
    // assign this field when a possession bullet obj is initialized.
    [SerializeField] private GameObject shooter;
    [SerializeField] private List<GameObject> possessionPair;
    
    [Header("Properties Assigned by Shooter")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector3 moveDir;

    private void OnEnable()
    {
        rb = this.GetComponent<Rigidbody>();
        Invoke( "RecycleObj", lifeCycle);
    }
    
    private void FixedUpdate()
    {
        if (!isBulletReady) return;
        rb.MovePosition(transform.position + moveDir * bulletSpeed * Time.deltaTime);
    }
    
    
    /// <summary>
    /// Recycle bullet into objectpool for later use.
    /// </summary>
    void RecycleObj()
    {
        shooter = null;
        possessionPair.Clear();
        isBulletReady = false;
        rb.velocity = Vector3.zero;
        ObjPoolManager.GetInstance().PushObj(this.gameObject.name, this.gameObject);
    }

    /// <summary>
    /// Process main logic happens after bullet hitting anything.
    /// Hitting enemy will initiate the possession event.
    /// Hitting anything except a transparent wall causes the bullet to disappear afterwards. 
    /// </summary>
    /// <param name="other">The object that the bullet collides with.</param>
    private void OnCollisionEnter(Collision other)
    {
        // enter the sequence only if the enemy hit is an enemy.
        //TODO: Integrate with enemy scripts to see if this enemy is dead.
        if  ( other.collider.CompareTag("Enemy") )
        {
            Debug.Log("Possession bullet hits an enemy");
            possessionPair.Add(shooter);
            possessionPair.Add(other.gameObject);
            EventCenter.GetInstance().TriggerEvent("PossessionSequence", possessionPair );
        }
        else if (other.collider.CompareTag("Key"))
        {
            Debug.Log("Possession bullet hits a key on an enemy");
            possessionPair.Add(shooter);
            // Add the key's parent enemy gameobject to the possession pair as the hit (possessed) one.
            Transform enemyTransform = other.gameObject.transform.parent;
            if(enemyTransform != null)
            {
                GameObject EnemyObj = enemyTransform.gameObject;
               possessionPair.Add(EnemyObj);
            }
        }
        
        // After collision, recycle this object immediately
        // except colliding with a transparent wall or a detect area
        if (!other.collider.CompareTag("TransparentWall") && !other.collider.CompareTag("DetectArea")  )
            RecycleObj();
    }

    /// <summary>
    /// Assign shooter, fly speed and shooting direction when a bullet is initialized in the CharController script. Also resets its position and velocity before enables FixedUpdate().
    /// </summary>
    /// <param name="obj">Shooter of this bullet; a game object contains CharController component</param>
    /// <param name="targetDir">Target direction of this bullet, which equals the facing direction of the shooter when the shot is fired.</param>
    /// <param name="speed">Speed of the bullet. </param>
    public void InitializePossessionBullet(GameObject obj, Vector3 targetDir, float speed)
    {
        this.shooter = obj;
        this.bulletSpeed = speed;
        this.moveDir = targetDir;
        this.transform.position = shooter.transform.position + targetDir * 1.5f;
        this.rb.velocity = Vector3.zero;
        //Debug.Log("InitalizePosessionBullet is executed.");
        isBulletReady = true;
    }
}
