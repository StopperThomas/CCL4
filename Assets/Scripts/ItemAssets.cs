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
}
