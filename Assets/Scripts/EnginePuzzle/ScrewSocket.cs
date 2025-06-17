using UnityEngine;

public class ScrewSocket : MonoBehaviour
{
    public ScrewType requiredType;
    public Transform wireConnectionPoint;

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

            PuzzleManager.Instance.NotifyScrewPlaced(this); // Optional if puzzle manager is involved

            return true;
        }

        return false;
    }

    public Transform GetConnectionPoint()
    {
        return wireConnectionPoint != null ? wireConnectionPoint : transform;
    }

    public void RemoveScrew()
{
    if (placedScrew != null)
    {
        // Add the screw back to inventory
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

}
