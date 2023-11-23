using UnityEngine;

// This sprite can hint players which character they are controlling right now
public class FollowPlayerSprite : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private PossessionManager possessionManager;

    private GameObject originalPlayer;

    void Start()
    {
        possessionManager = FindObjectOfType<PossessionManager>();
        if (possessionManager == null)
        {
            Debug.Log("PossessionManager not found in the scene.");
        }
        originalPlayer = possessionManager.originalPlayer;
        target = originalPlayer.transform;
    }



    void Update()
    {
        // To see if control target change
        UpdateTarget();

        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = target.rotation;
        }
    }

    private void UpdateTarget()
    {
        GameObject currentPossessedEnemy = possessionManager.GetCurrentPossessedEnemy();
        if (currentPossessedEnemy != null)
        {
            target = currentPossessedEnemy.transform; // if there's possessed enemy, move the sprite to the enemy bottom
        }
        else
        {
            target = originalPlayer.transform;
        }
    }
}

