using UnityEngine;

public class Screw : MonoBehaviour
{
    public string screwID;
    public bool isUnscrewed = false;

    void Start()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsScrewUnscrewed(screwID))
        {
            isUnscrewed = true;
            gameObject.SetActive(false);
        }
    }

   public bool TryUnscrew(Item equippedItem)
{
    if (isUnscrewed || equippedItem == null) return false;

    var pickup = GetComponent<PickupItem>();
    if (pickup == null) return false;

    if (equippedItem.itemType == ItemType.ScrewDriver &&
        (ScrewType)equippedItem.screwdriverType == pickup.screwType)
    {
        isUnscrewed = true;
        GameStateManager.Instance?.MarkScrewUnscrewed(screwID);
        InventoryManager.Instance.inventory.AddItem(pickup.GetItem());
        InventoryManager.Instance.uiInventory.RefreshInventoryItems();
        gameObject.SetActive(false);
        return true;
    }

    return false;
}

}
