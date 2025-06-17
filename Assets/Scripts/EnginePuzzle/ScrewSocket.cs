using UnityEngine;

public class ScrewSocket : MonoBehaviour
{
    public ScrewType requiredType;

    private GameObject placedScrew;
    private bool isFilled = false;

    public bool IsFilled => isFilled;
    public ScrewType PlacedType { get; private set; }

    public bool TryPlaceScrew(GameObject screwPrefab, ScrewType type)
    {
        if (isFilled || screwPrefab == null) return false;

        if (type == requiredType)
        {
            placedScrew = Instantiate(screwPrefab, transform.position, transform.rotation);
            placedScrew.transform.localScale = Vector3.one;
            placedScrew.tag = "Untagged";
            isFilled = true;
            PlacedType = type;

            PuzzleManager.Instance?.NotifyScrewPlaced(this);
            return true;
        }

        return false;
    }

    public void RemoveScrew()
    {
        if (placedScrew != null)
        {
            // Return the screw to the inventory
            Item screwItem = new Item
            {
                itemType = ItemType.Screw,
                screwType = PlacedType,
                amount = 1,
                prefab3D = ItemAssets.Instance.GetScrewPrefab(PlacedType)
            };

            InventoryManager.Instance.inventory.AddItem(screwItem);

            Destroy(placedScrew);
            placedScrew = null;
            isFilled = false;
            PlacedType = default;
        }
    }

    public void ResetSocket()
    {
        if (isFilled && placedScrew != null)
        {
            Destroy(placedScrew);
        }

        isFilled = false;
        PlacedType = default;
    }

}
