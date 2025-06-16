using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite screwDriverSprite;
    public Sprite gearSprite;
    public Sprite screwSprite;
    public Sprite paperSprite;

    public GameObject screwDriverPrefab;
    public GameObject gearPrefab;
    public GameObject screwPrefab;
    public GameObject paperPrefab;

    public GameObject GetPrefab(Item.ItemType itemType)
    {
        switch (itemType)
        {
            case Item.ItemType.ScrewDriver: return screwDriverPrefab;
            case Item.ItemType.Gear: return gearPrefab;
            case Item.ItemType.Screw: return screwPrefab;
            case Item.ItemType.Paper: return paperPrefab;
            default: return null;
        }
    }
}

