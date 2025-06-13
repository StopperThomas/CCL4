using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private HashSet<string> unscrewedScrews = new HashSet<string>();
    private string equippedScrewdriverID = null;

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

    // --- Screws ---
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

    // --- Equipped Screwdriver ---
    public void SetEquippedScrewdriver(string screwdriverID)
    {
        equippedScrewdriverID = screwdriverID;
        Debug.Log($"Equipped screwdriver set to {screwdriverID}");
    }

    public string GetEquippedScrewdriver()
    {
        return equippedScrewdriverID;
    }
}
