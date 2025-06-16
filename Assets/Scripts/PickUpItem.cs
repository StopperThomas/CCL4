using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Item.ItemType itemType;
    public int amount = 1;

    [Header("Optional Custom Data")]
    public string customName;
    [TextArea] public string customDescription;
    public GameObject custom3DPrefab;

    public Item GetItem()
    {
        Item item = new Item
        {
            itemType = itemType,
            amount = amount,
            itemName = string.IsNullOrEmpty(customName) ? itemType.ToString() : customName,
            description = string.IsNullOrWhiteSpace(customDescription)
                ? "Description for " + itemType
                : customDescription,
            prefab3D = ItemAssets.Instance.GetPrefab(itemType)
        };

        // Special handling for screwdrivers
        if (itemType == Item.ItemType.ScrewDriver)
        {
            Screwdriver screwdriverComponent = GetComponent<Screwdriver>();
            if (screwdriverComponent != null)
            {
                item.screwType = screwdriverComponent.screwType;
            }
            else
            {
                Debug.LogWarning("Screwdriver component missing on PickupItem.");
            }
        }

        return item;
    }

    public void OnPickup()
    {
        InventoryManager.Instance.inventory.AddItem(GetItem());

        UI_Inventory uiInventory = FindObjectOfType<UI_Inventory>();
        if (uiInventory != null)
        {
            uiInventory.SetInventory(InventoryManager.Instance.inventory);
        }

        Destroy(gameObject);
    }
}
