using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float viewDistance = 5;
    public float viewAngle = 90;
    public Vector3 initialPosition;
    private float lastDetectionTime = -5.0f;
    private float detectionCooldown = 1.0f; // 设定一个1秒的冷却时间

    private void Update()
    {
        DetectPlayer();
    }

    private void Start()
    {
        initialPosition = transform.position;

        DrawDetectArea(transform, viewAngle, viewDistance);
    }

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
        
        // Add a tag for possession bullet to distinguish
        go.tag = "DetectArea";
    }
    void DetectPlayer()
    {
        if (Time.time - lastDetectionTime < detectionCooldown)
            return;

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

    void OnPlayerSpotted(CharController player)
    {
        CharacterHealth playerHealth = player.GetComponent<CharacterHealth>();

        if (player && playerHealth)
        {
            switch (player.currentState)  // Assuming you've defined currentState in CharController
            {
                case CharController.PlayerState.Normal:
                    playerHealth.IsExposed();
                    player.TeleportToStart();  // Assuming you've defined TeleportToStart in CharController
                    break;
                case CharController.PlayerState.Possessing:
                    break;
                case CharController.PlayerState.Fighting:
                    playerHealth.HitByBullet(0.5f);
                    break;
            }
        }
    }


    public void ReturnToPosition()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent)
        {
            agent.SetDestination(initialPosition);
        }
    }
}
