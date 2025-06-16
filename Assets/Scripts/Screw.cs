using UnityEngine;

public class Screw : MonoBehaviour
{
    public string screwID;
    public ScrewType screwType;
    public bool isUnscrewed = false;

    public int amount = 1;

    void Start()
    { Debug.Log($"Screw {name} has screwType: {screwType}");

        if (GameStateManager.Instance != null && GameStateManager.Instance.IsScrewUnscrewed(screwID))
        {
            isUnscrewed = true;
            gameObject.SetActive(false);
        }
    }

    public void TryUnscrew(Item equippedItem)
    {
        if (isUnscrewed) return;

        if (equippedItem == null)
        {
            Debug.LogWarning("No item equipped!");
            return;
        }

        // Debug info
        Debug.Log($"[Screw.cs] Attempting to unscrew:");
        Debug.Log($"  Screw ID: {screwID}");
        Debug.Log($"  Screw Type: {screwType}");
        Debug.Log($"  Equipped Item: {equippedItem.itemName}, Type: {equippedItem.itemType}, ScrewType: {equippedItem.screwType}");

        if (equippedItem.itemType == Item.ItemType.ScrewDriver && equippedItem.screwType == screwType)
        {
            Debug.Log("Correct screwdriver! Unscrewing...");
            isUnscrewed = true;

            GameStateManager.Instance?.MarkScrewUnscrewed(screwID);

            // Add screw to inventory
            Item screwItem = new Item
            {
                itemType = Item.ItemType.Screw,
                amount = amount,
                itemName = "Screw",
                description = "A regular screw.",
                prefab3D = ItemAssets.Instance.screwPrefab,
                screwType = screwType
            };

            InventoryManager.Instance.inventory.AddItem(screwItem);
            InventoryManager.Instance.uiInventory.RefreshInventoryItems();

            gameObject.SetActive(false);
            return;
        }

        Debug.Log("Wrong tool. Try a different one.");
    }
}
