using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Screwdriver sprites by type
    public Sprite screwDriverMinusSprite;
    public Sprite screwDriverPlusSprite;
    public Sprite screwDriverTorxSprite;
    public Sprite screwDriverYSprite;
    public Sprite screwDriverFlatSprite;

    // Screw sprites by type
    public Sprite screwMinusSprite;
    public Sprite screwPlusSprite;
    public Sprite screwTorxSprite;
    public Sprite screwYSprite;

    // Key sprites
    public Sprite keyBoxSprite;
    public Sprite keyDoorSprite;

    // Cogwheel sprites
    public Sprite cogwheelSmallSprite;
    public Sprite cogwheelMediumSprite;
    public Sprite cogwheelLargeSprite;

    // Note sprites
    public Sprite note0Sprite;
    public Sprite note1Sprite;
    public Sprite note2Sprite;

    // Screwdrivers
    public GameObject screwDriverMinusPrefab;
    public GameObject screwDriverPlusPrefab;
    public GameObject screwDriverTorxPrefab;
    public GameObject screwDriverYPrefab;
    public GameObject screwDriverFlatPrefab;

    // Screws
    public GameObject screwMinusPrefab;
    public GameObject screwPlusPrefab;
    public GameObject screwTorxPrefab;
    public GameObject screwYPrefab;

    // Keys
    public GameObject keyBoxPrefab;
    public GameObject keyDoorPrefab;

    // Cogwheels
    public GameObject cogwheelSmallPrefab;
    public GameObject cogwheelMediumPrefab;
    public GameObject cogwheelLargePrefab;

    // Notes
    public GameObject note0Prefab;
    public GameObject note1Prefab;
    public GameObject note2Prefab;

    // LightBulb
    public Sprite bulbSprite;
    public GameObject bulbPrefab;


    public Sprite GetScrewdriverSprite(ScrewdriverType type)
    {
        return type switch
        {
            ScrewdriverType.Minus => screwDriverMinusSprite,
            ScrewdriverType.Plus => screwDriverPlusSprite,
            ScrewdriverType.Torx => screwDriverTorxSprite,
            ScrewdriverType.Y => screwDriverYSprite,
            ScrewdriverType.Flat => screwDriverFlatSprite,
            _ => null,
        };
    }

    public Sprite GetScrewSprite(ScrewType type)
    {
        return type switch
        {
            ScrewType.Minus => screwMinusSprite,
            ScrewType.Plus => screwPlusSprite,
            ScrewType.Torx => screwTorxSprite,
            ScrewType.Y => screwYSprite,
            _ => null,
        };
    }

    public Sprite GetKeySprite(KeyType type)
    {
        return type switch
        {
            KeyType.Box => keyBoxSprite,
            KeyType.Door => keyDoorSprite,
            _ => null,
        };
    }

    public Sprite GetCogwheelSprite(CogwheelType type)
    {
        return type switch
        {
            CogwheelType.Small => cogwheelSmallSprite,
            CogwheelType.Medium => cogwheelMediumSprite,
            CogwheelType.Large => cogwheelLargeSprite,
            _ => null,
        };
    }

    public Sprite GetNoteSprite(int noteID)
    {
        return noteID switch
        {
            0 => note0Sprite,
            1 => note1Sprite,
            2 => note2Sprite,
            _ => null,
        };
    }


    public GameObject GetScrewdriverPrefab(ScrewdriverType type)
    {
        return type switch
        {
            ScrewdriverType.Minus => screwDriverMinusPrefab,
            ScrewdriverType.Plus => screwDriverPlusPrefab,
            ScrewdriverType.Torx => screwDriverTorxPrefab,
            ScrewdriverType.Y => screwDriverYPrefab,
            ScrewdriverType.Flat => screwDriverFlatPrefab,
            _ => null,
        };
    }

public GameObject GetScrewPrefab(ScrewType type)
{
    GameObject prefab = type switch
    {
        ScrewType.Minus => screwMinusPrefab,
        ScrewType.Plus => screwPlusPrefab,
        ScrewType.Torx => screwTorxPrefab,
        ScrewType.Y => screwYPrefab,
        _ => null,
    };

    if (prefab == null)
        Debug.LogWarning($"Missing prefab for ScrewType: {type}");

    return prefab;
}


    public GameObject GetKeyPrefab(KeyType type)
    {
        return type switch
        {
            KeyType.Box => keyBoxPrefab,
            KeyType.Door => keyDoorPrefab,
            _ => null,
        };
    }

    public GameObject GetCogwheelPrefab(CogwheelType type)
    {
        return type switch
        {
            CogwheelType.Small => cogwheelSmallPrefab,
            CogwheelType.Medium => cogwheelMediumPrefab,
            CogwheelType.Large => cogwheelLargePrefab,
            _ => null,
        };
    }

    public GameObject GetNotePrefab(int noteID)
    {
        return noteID switch
        {
            0 => note0Prefab,
            1 => note1Prefab,
            2 => note2Prefab,
            _ => null,
        };
    }

    public GameObject GetPrefab(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.ScrewDriver => screwDriverMinusPrefab,
            ItemType.Cogwheel => gearPrefab,
            ItemType.Screw => screwMinusPrefab,
            ItemType.Note => note0Prefab,
            
            _ => null,
        };
    }

    // Legacy fallback
    public GameObject gearPrefab; // optional fallback for cogwheel
}
