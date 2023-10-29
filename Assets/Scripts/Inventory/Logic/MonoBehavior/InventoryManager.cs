using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;

    public InventoryData_SO actionData;

    public InventoryData_SO equipmentData;

    [Header("ContainerS")]

    public ContainerUI inventoryUI;

    public ContainerUI actionUI;

    public ContainerUI equipmentUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDrag;



    [Header("Status Text")]
    public Text healthText;

    public Text speedText;

    public Text style;

    public Text skill;

    [Header("GreenCount Text")]
    public Text greenCountText;

    [Header("RedCount Text")]
    public Text redCountText;

    [Header("GrayCount Text")]
    public Text grayCountText;


    public void UpdateStatusText(int health, int speed)
    {
        //healthText.text
    }

    void Start()
    {
        foreach (var item in inventoryData.items)
        {
            item.itemData = null;
            item.amount = 0;
        }
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    public bool CheckInInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
}

public abstract class Singleton<T> : Singleton where T : MonoBehaviour
{
    #region  Fields
    [CanBeNull]
    private static T _instance;

    [NotNull]
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object Lock = new object();

    [SerializeField]
    private bool _persistent = true;
    #endregion

    #region  Properties
    [NotNull]
    public static T Instance
    {
        get
        {
            if (Quitting)
            {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                // ReSharper disable once AssignNullToNotNullAttribute
                return null;
            }
            lock (Lock)
            {
                if (_instance != null)
                    return _instance;
                var instances = FindObjectsOfType<T>();
                var count = instances.Length;
                if (count > 0)
                {
                    if (count == 1)
                        return _instance = instances[0];
                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                    for (var i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return _instance = instances[0];
                }

                Debug.Log($"[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                return _instance = new GameObject($"({nameof(Singleton)}){typeof(T)}")
                           .AddComponent<T>();
            }
        }
    }
    #endregion

    #region  Methods
    private void Awake()
    {
        if (_persistent)
            DontDestroyOnLoad(gameObject);
        OnAwake();
    }

    protected virtual void OnAwake() { }
    #endregion
}

public abstract class Singleton : MonoBehaviour
{
    #region  Properties
    public static bool Quitting { get; private set; }
    #endregion

    #region  Methods
    private void OnApplicationQuit()
    {
        Quitting = true;
    }
    #endregion
}
