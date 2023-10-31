using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;

    private KeyAttribute keyAttribute;
    private bool keyIsPickedUp;

    private void Awake()
    {
        keyAttribute = GetComponent<KeyAttribute>();
        if (keyAttribute != null)
        {
            keyIsPickedUp = keyAttribute.IsKeyPickedUp;
            keyAttribute.OnKeyDropped += HandleKeyDropped;// subscribe the event
            keyAttribute.OnKeyPickedUp += HandleKeyPickedUp;
        }
    }



    private void HandleKeyDropped()
    {
        keyIsPickedUp = false;
        Debug.Log("Key was dropped and the player can pick it up now.");
    }

    private void HandleKeyPickedUp()
    {
        keyIsPickedUp = true;
        Debug.Log("Key was picked up and the player cannot pick it up now.");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&!keyIsPickedUp)
        {
            //TODO:Add item to bag
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //TODO:eq item

            Destroy(gameObject);
        
        }
    }

    private void OnDestroy()
    {
        if (keyAttribute != null)
        {
            keyAttribute.OnKeyDropped -= HandleKeyDropped;
            keyAttribute.OnKeyPickedUp -= HandleKeyPickedUp;
        }
    }
}
