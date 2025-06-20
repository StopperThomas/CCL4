using UnityEngine;

public class Item
{
    public ItemType itemType;
    public int amount;

    public string itemName;
    public string description;
    public GameObject prefab3D;

    public float customRenderDistance = -1f; 
    public float customRenderScale = 1f;

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
        ItemType.LightBulb => ItemAssets.Instance.bulbSprite,
         _=> null // catch-all to satisfy compiler
    };
}



    public GameObject GetPrefab()
{
    if (prefab3D != null)
        return prefab3D;

    prefab3D = itemType switch
    {
        ItemType.ScrewDriver => ItemAssets.Instance.GetScrewdriverPrefab(screwdriverType),
        ItemType.Screw => ItemAssets.Instance.GetScrewPrefab(screwType),
        ItemType.Key => ItemAssets.Instance.GetKeyPrefab(keyType),
        ItemType.Cogwheel => ItemAssets.Instance.GetCogwheelPrefab(cogwheelType),
        ItemType.LightBulb => ItemAssets.Instance.bulbPrefab,
         _=> null,
    };

    return prefab3D;
}

}