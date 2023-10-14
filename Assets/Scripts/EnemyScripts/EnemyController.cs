using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float viewDistance = 5;  // Detection range for the enemy's vision
    public float viewAngle = 90;  // Angle of the enemy's field of view
    public Vector3 initialPosition;  // The initial position where the enemy starts
    private float lastDetectionTime = -5.0f;
    private float detectionCooldown = 1.0f; // Cooldown time between detections
    
    public NormalBullet normalBulletPrefab; // Prefab for the bullet to be shot by the enemy
    public Transform firePoint;  // Point from which the bullet is shot
    private Vector3 lastSpottedPlayerPosition;  // The last position where the player was spotted
    public float defaultBulletSpeed = 5.0f;  // Default speed for the bullet
    public float defaultBulletDamage = 1.0f;   // Default damage dealt by the bullet

    // Continuously check for the player's presence within the enemy's field of view
    private void Update()
    {
        DetectPlayer();
    }

    private void Start()
    {
        initialPosition = transform.position;  // Set the enemy's starting position

        DrawDetectArea(transform, viewAngle, viewDistance);  // Draw the enemy's field of view visualization in the game world
    }

    // Method to draw the enemy's field of view
    public void DrawDetectArea(Transform t, float angle, float radius)
    {
        int segments = 100;
        float deltaAngle = angle / segments;
        Vector3 forward = t.forward;

        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = t.position;
        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, -angle / 2 + deltaAngle * (i - 1), 0f) * forward * radius + t.position;
            vertices[i] = pos;
        }
        int trianglesAmount = segments;
        int[] triangles = new int[segments * 3];
        for (int i = 0; i < trianglesAmount; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = i + 2;
        }

        GameObject go = new GameObject("DetectArea");
        go.transform.position = new Vector3(0, 0.1f, 0);
        go.transform.SetParent(transform);
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        mr.material.shader = Shader.Find("Unlit/Color");
        mr.material.color = new Color(1.0f, 0.843f, 0.0f, 0.5f);
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mf.mesh = mesh;
    }
    
    // Method to detect the player within the enemy's field of view
    void DetectPlayer()
    {
        // If detection cooldown hasn't passed, exit method
        if (Time.time - lastDetectionTime < detectionCooldown)
            return;

        // Look for the player's game object
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            CharController player = playerObject.GetComponent<CharController>();
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            if (angle < viewAngle / 2.0f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dirToPlayer, out hit, viewDistance))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        lastDetectionTime = Time.time; // 更新最后检测时间
                        OnPlayerSpotted(player);
                    }
                }
            }
        }
    }

    // Actions to be performed when a player is detected
    void OnPlayerSpotted(CharController player)
    {
        lastSpottedPlayerPosition = player.transform.position;
        CharacterHealth playerHealth = player.GetComponent<CharacterHealth>();

        if (player && playerHealth)
        {
            switch (player.currentState)  // Assuming you've defined currentState in CharController
            {
                case CharController.PlayerState.Normal:
                    playerHealth.curHealth -= 1;
                    player.TeleportToStart();  // Assuming you've defined TeleportToStart in CharController
                    break;
                case CharController.PlayerState.Possessing:
                    break;
                case CharController.PlayerState.Fighting:
                    playerHealth.curHealth -= Mathf.FloorToInt(0.5f);
                    break;
            }
        }
        
        Vector3 targetDir = (lastSpottedPlayerPosition - transform.position).normalized;
        ShootNormalBulletAtPlayer(targetDir, defaultBulletSpeed, defaultBulletDamage);
    }


    // Command the enemy to return to its starting position
    public void ReturnToPosition()
    {
        // Use Unity's NavMeshAgent to move the enemy back to its initial position
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent)
        {
            agent.SetDestination(initialPosition);
        }
    }
    
    // Shoot a bullet at the detected player
    private void ShootNormalBulletAtPlayer(Vector3 targetDir, float bulletSpeed, float damageAmount)
    {
        GameObject bullet = ObjPoolManager.GetInstance().GetObj("Prefabs/NormalBullet");
        if (bullet == null)
        {
            Debug.LogError("Failed to get bullet from ObjPoolManager");
            return;
        }

        // Initialize the bullet using the enemy's position and offset slightly to avoid immediate collision with the launcher
        bullet.transform.position = this.transform.position + targetDir.normalized * 1.5f;
    
        NormalBullet normalBullet = bullet.GetComponent<NormalBullet>();
        normalBullet.InitializeNormalBullet(targetDir, bulletSpeed, damageAmount);
    }

}
