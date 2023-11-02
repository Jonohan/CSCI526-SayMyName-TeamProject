using UnityEngine;
using UnityEngine.UI;

public class FollowCharacterUI : MonoBehaviour
{
    public Transform targetCharacter;  
    public RectTransform uiElement;    
    public Vector3 offset;             

    private Camera mainCamera;
    private bool isCharacterInside = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            isCharacterInside = true;
            uiElement.gameObject.SetActive(true);  
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            isCharacterInside = false;
            uiElement.gameObject.SetActive(false);  
        }
    }
}

