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
            PromptManager.Instance?.ShowPrompt("You need to equip a key to use it.");
            return;
        }

        if (equippedItem.keyType != requiredKeyType)
        {
            PromptManager.Instance?.ShowPrompt("This key doesn't fit.");
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

        PromptManager.Instance?.ShowPrompt("The key clicks and the lock opens.");
    }

    public bool IsUnlocked() => isUnlocked;
}
