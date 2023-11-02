using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
    public Transform targetCharacter;
    public RectTransform uiElement;
    public Vector3 offset;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (targetCharacter == null)
            return;

        // Converts a character's 3D world coordinates to screen coordinates
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetCharacter.position + offset);

        uiElement.position = screenPos;
    }

}

