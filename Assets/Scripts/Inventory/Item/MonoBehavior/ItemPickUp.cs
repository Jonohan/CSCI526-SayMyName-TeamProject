using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //TODO:Add item to bag
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //TODO:eq item

            Destroy(gameObject);
        
        }
    }
}
