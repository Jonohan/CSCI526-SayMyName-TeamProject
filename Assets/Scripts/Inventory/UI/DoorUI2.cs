using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUI2 : MonoBehaviour
{
    public GameObject keyObject;
    public void OpenDoor()
    {
        StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + new Vector3(0, -2, 0); // Door move down to open

        float duration = 1.5f; // Duration of opening door
        float elapsed = 0f;

        // Open the door smoothly
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    // Open the door with key
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ensure InventoryManager instance and inventoryData are not null
            if (InventoryManager.Instance != null && InventoryManager.Instance.inventoryData != null)
            {
                foreach (var item in InventoryManager.Instance.inventoryData.items)
                {
                    // Ensure item and itemData are not null
                    if (item != null && item.itemData != null && (item.itemData.itemName == "SilverKey"|| item.itemData.itemName == "Silver Key"))
                    {
                        OpenDoor();
                    }
                }
            }
        }
        if (other.gameObject == keyObject)
        {
            OpenDoor();
        }
    }
}
