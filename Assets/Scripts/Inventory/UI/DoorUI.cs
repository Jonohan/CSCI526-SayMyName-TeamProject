using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUI : MonoBehaviour
{
    public GameObject keyObject;
    public void OpenDoor()
    {
        StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + new Vector3(0, -3, 0); // Door move down to open

        float duration = 2f; // Duration of opening door
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
            foreach (var item in InventoryManager.Instance.inventoryData.items)
            {
                if (item.itemData.itemName == "Key2")
                {
                    OpenDoor();
                }
            }
        }
        if (other.gameObject == keyObject)
        {
            OpenDoor();
        }
    }

}
