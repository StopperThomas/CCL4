using UnityEngine;

public class Item
{
    public ItemType itemType;
    public int amount;

    public string itemName;
    public string description;
    public GameObject prefab3D;

    public ScrewdriverType screwdriverType;
    public ScrewType screwType;
    public KeyType keyType;
    public CogwheelType cogwheelType;
    public int noteID;

   public Sprite GetSprite()
{
    return itemType switch
    {
        ItemType.ScrewDriver => ItemAssets.Instance.GetScrewdriverSprite(screwdriverType),
        ItemType.Screw => ItemAssets.Instance.GetScrewSprite(screwType),
        ItemType.Key => ItemAssets.Instance.GetKeySprite(keyType),
        ItemType.Cogwheel => ItemAssets.Instance.GetCogwheelSprite(cogwheelType),
        ItemType.Note => ItemAssets.Instance.GetNoteSprite(noteID),
        _ => null,
    };
}


    public GameObject GetPrefab()
    {
        switch (itemType)
        {
            case ItemType.ScrewDriver: return ItemAssets.Instance.GetScrewdriverPrefab(screwdriverType);
            case ItemType.Screw: return ItemAssets.Instance.GetScrewPrefab(screwType);
            case ItemType.Key: return ItemAssets.Instance.GetKeyPrefab(keyType);
            case ItemType.Cogwheel: return ItemAssets.Instance.GetCogwheelPrefab(cogwheelType);
            case ItemType.Note: return ItemAssets.Instance.GetNotePrefab(noteID);
            default: return null;
        }
    }
}

