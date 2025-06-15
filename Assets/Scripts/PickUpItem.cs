using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Item.ItemType itemType;
    public int amount = 1;

public Item GetItem()
{
    Item item = new Item
    {
        itemType = itemType,
        amount = amount,
        itemName = itemType.ToString(),
        description = "Description for " + itemType,
        prefab3D = null 
    };

    item.prefab3D = item.GetPrefab(); 
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
