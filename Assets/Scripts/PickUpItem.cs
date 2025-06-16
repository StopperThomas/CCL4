using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemType itemType;
    public int amount = 1;

    [Header("Variant Type")]
    public ScrewdriverType screwdriverType;
    public ScrewType screwType;
    public KeyType keyType;
    public CogwheelType cogwheelType;
    public int noteID;

    [Header("Optional Custom Data")]
    public string customName;
    [TextArea] public string customDescription;

    public void OnPickup()
    {
        Item item = GetItem();
        InventoryManager.Instance.inventory.AddItem(item);

        UI_Inventory uiInventory = FindObjectOfType<UI_Inventory>();
        if (uiInventory != null)
        {
            uiInventory.SetInventory(InventoryManager.Instance.inventory);
        }

        Destroy(gameObject); // Remove item from scene
    }

    public Item GetItem()
    {
        Item item = new Item
        {
            itemType = itemType,
            amount = amount,
            itemName = string.IsNullOrEmpty(customName) ? itemType.ToString() : customName,
            description = string.IsNullOrWhiteSpace(customDescription) ? $"Description for {itemType}" : customDescription
        };

        switch (itemType)
        {
            case ItemType.ScrewDriver:
                item.screwdriverType = screwdriverType;
                item.prefab3D = ItemAssets.Instance.GetScrewdriverPrefab(screwdriverType);
                break;
            case ItemType.Screw:
                item.screwType = screwType;
                item.prefab3D = ItemAssets.Instance.GetScrewPrefab(screwType);
                break;
            case ItemType.Key:
                item.keyType = keyType;
                item.prefab3D = ItemAssets.Instance.GetKeyPrefab(keyType);
                break;
            case ItemType.Cogwheel:
                item.cogwheelType = cogwheelType;
                item.prefab3D = ItemAssets.Instance.GetCogwheelPrefab(cogwheelType);
                break;
            case ItemType.Note:
                item.noteID = noteID;
                item.prefab3D = ItemAssets.Instance.GetNotePrefab(noteID);
                break;
            default:
                item.prefab3D = ItemAssets.Instance.GetPrefab(itemType);
                break;
        }

        return item;
    }
}
