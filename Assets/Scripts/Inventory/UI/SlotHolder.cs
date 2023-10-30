using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType {BAG,KEY,ARMOR,BOOT,ACTION}
public class SlotHolder : MonoBehaviour
{
    public SlotType slotType;
    
    public ItemUI itemUI;

    public void UpdateItem()
    {
        switch(slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;

            case SlotType.KEY:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
                
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
                
            case SlotType.BOOT:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;

            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
        }
        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetupItemUI(item.itemData, item.amount);
    }
}
