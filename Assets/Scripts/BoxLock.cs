using UnityEngine;

public class BoxLock : MonoBehaviour
{
    [SerializeField] private KeyType requiredKeyType = KeyType.Box;
    private bool isUnlocked = false;

    public void TryUnlock(Item equippedItem)
    {
        if (isUnlocked) return;

        if (equippedItem == null || equippedItem.itemType != ItemType.Key)
        {
            FindObjectOfType<InspectUIManager>()?.ShowPrompt(true, "You need to equip a key to use it.", true);
            return;
        }

        if (equippedItem.keyType != requiredKeyType)
        {
            FindObjectOfType<InspectUIManager>()?.ShowPrompt(true, "This key doesn't fit.", true);
            Debug.Log("Wrong key type.");
            return;
        }

        Debug.Log("Lock opened with key: " + equippedItem.itemName);
        isUnlocked = true;

        // Despawn lock
        gameObject.SetActive(false);

        // Remove key from inventory
        InventoryManager.Instance.inventory.RemoveItem(equippedItem);
        InventoryManager.Instance.uiInventory.RefreshInventoryItems();

        // Clear equipped item slot
        InventoryManager.Instance.uiInventory.UpdateEquippedSlot(null);

        // Unequip from interaction system
        var interaction = FindObjectOfType<InteractionManager>();
        interaction?.EquipFromUI(null);

        // If key is being inspected, hide it
        var inspectorUI = FindObjectOfType<ItemInspectorUI>();
        if (inspectorUI != null && inspectorUI.GetCurrentItem() == equippedItem)
        {
            inspectorUI.HideItem();
        }

        FindObjectOfType<InspectUIManager>()?.ShowPrompt(true, "The key clicks and the lock opens.", true);
    }

    public bool IsUnlocked() => isUnlocked;
}
