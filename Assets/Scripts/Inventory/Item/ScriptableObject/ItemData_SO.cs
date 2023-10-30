using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Key, Armor, Boot, Useable}
[CreateAssetMenu(fileName ="New Item",menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    [TextArea]
    public string description = "";
    public bool stackable;

    [Header("Key")]
    public GameObject KeyPrefab;
    [Header("Armor")]
    public GameObject ArmorPrefab;
    [Header("Boot")]
    public GameObject BootPrefab;
    [Header("Useable")]
    public GameObject UseablePrefab;
}
