using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
            Debug.Log(other.collider);
            Debug.Log("Damage bullet hits an enemy");
            GameObject enemy = other.collider.gameObject;  // get the enemy gameobject\
            enemy.GetComponent<EnemyHealthController>().restHealth--;
            Debug.Log("Enemy health = " + enemy.GetComponent<EnemyHealthController>().restHealth);
            //Transform canvas = enemy.transform.GetChild(2); // get the canvas->frontground
            //Image healthBarFrontground = canvas.GetChild(1).GetComponent<Image>();
            //Debug.Log("healthBarFrontground is " + healthBarFrontground.name);
            //healthBarFrontground.fillAmount = 1.0f * enemy.GetComponent<EnemyHealthController>().restHealth / enemy.GetComponent<EnemyHealthController>().maxHealth;
            if (other.gameObject.GetComponent<EnemyHealthController>().restHealth <= 0)
            {
                Destroy(other.gameObject);
                EventCenter.GetInstance().TriggerEvent("KilledOneEnemy");
            }
                
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
