using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        ScrewDriver,
        Gear,
        Screw,
        Paper
    }
    public ItemType itemType;
    public int amount;

    public string itemName;
    public string description;
    public GameObject prefab3D;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            case ItemType.ScrewDriver:
                return ItemAssets.Instance.screwDriverSprite;
            case ItemType.Gear:
                return ItemAssets.Instance.gearSprite;
            case ItemType.Screw:
                return ItemAssets.Instance.screwSprite;
            case ItemType.Paper:
                return ItemAssets.Instance.paperSprite;
            default:
                return null;
        }
    }

    public GameObject GetPrefab()
    {
        switch (itemType)
        {
            case ItemType.ScrewDriver: return ItemAssets.Instance.screwDriverPrefab;
            case ItemType.Gear: return ItemAssets.Instance.gearPrefab;
            case ItemType.Screw: return ItemAssets.Instance.screwPrefab;
            case ItemType.Paper: return ItemAssets.Instance.paperPrefab;
            default: return null;
        }
    }



}
