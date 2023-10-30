using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DrugItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;

    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.Instance.CheckInActionUI(eventData.position) || InventoryManager.Instance.CheckInEquipmentUI(eventData.position) ||
            InventoryManager.Instance.CheckInInventoryUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent <SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }
                switch (targetHolder.slotType)
                {
                    case SlotType.BAG:
                        SwapItem();
                        break;
                    case SlotType.KEY:
                        if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Key)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.ARMOR:
                        if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.BOOT:
                        if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Boot)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.ACTION:
                        if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Useable)
                            {
                            SwapItem();
                            }
                        break;
                }
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);

        RectTransform t = transform as RectTransform;

        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }
    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

        bool isSameItem = tempItem.itemData == targetItem.itemData;

        if(isSameItem && targetItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }

    }
}
