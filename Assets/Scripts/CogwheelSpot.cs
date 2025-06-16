using UnityEngine;

public class CogwheelSpot : MonoBehaviour
{
    public CogwheelType requiredType;
    private bool isOccupied = false;

    // Call this when a cogwheel is placed
    public bool TryPlaceCogwheel(Item equippedItem)
    {
        if (isOccupied) return false;
        if (equippedItem == null || equippedItem.itemType != ItemType.Cogwheel) return false;
        if (equippedItem.cogwheelType != requiredType) return false;

        if (equippedItem.prefab3D != null)
        {
            Instantiate(equippedItem.prefab3D, transform.position, transform.rotation, transform);
        }

        isOccupied = true;

        if (equippedItem.amount > 1)
        {
            equippedItem.amount--;
        }
        else
        {
            InventoryManager.Instance.inventory.RemoveItem(equippedItem);
            FindObjectOfType<InteractionManager>()?.UnequipItem();
        }

        InventoryManager.Instance.uiInventory.RefreshInventoryItems();

        // ✅ Notify puzzle manager
        PuzzleDoor.Instance?.CheckPuzzleCompletion();

        return true;
    }

    // ✅ This method is what PuzzleDoor.cs expects to call
    public bool HasCorrectCogwheel()
    {
        return isOccupied;
    }
}
