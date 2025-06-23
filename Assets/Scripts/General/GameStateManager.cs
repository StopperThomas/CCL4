using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private HashSet<string> unscrewedScrews = new HashSet<string>();
    private string equippedItemID = null;  

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void MarkScrewUnscrewed(string screwID)
    {
        if (!unscrewedScrews.Contains(screwID))
        {
            unscrewedScrews.Add(screwID);
            Debug.Log($"Screw {screwID} marked unscrewed");
        }
    }

    public bool IsScrewUnscrewed(string screwID)
    {
        return unscrewedScrews.Contains(screwID);
    }

    
    public void SetEquippedItem(string itemID)
    {
        equippedItemID = itemID;
        Debug.Log($"Equipped item set to {itemID}");
    }

    public string GetEquippedItem()
    {
        return equippedItemID;
    }
}