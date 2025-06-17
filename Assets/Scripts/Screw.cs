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

   public void TryUnscrew(Item equippedItem)
{
    if (isUnscrewed || equippedItem == null) return;

    var pickup = GetComponent<PickupItem>();
    if (pickup == null)
    {
        Debug.LogWarning("No PickupItem attached to screw.");
        return;
    }

    // Validate the equipped screwdriver
    if (equippedItem.itemType == ItemType.ScrewDriver && (ScrewType)equippedItem.screwdriverType == pickup.screwType)

    {
        Debug.Log("Correct screwdriver! Unscrewing...");

        isUnscrewed = true;
        GameStateManager.Instance?.MarkScrewUnscrewed(screwID);

        InventoryManager.Instance.inventory.AddItem(pickup.GetItem());
        InventoryManager.Instance.uiInventory.RefreshInventoryItems();

        gameObject.SetActive(false);
    }
    else
    {
        Debug.Log("Wrong tool.");
    }
}

}
