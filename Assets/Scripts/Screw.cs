using UnityEngine;

public class Screw : MonoBehaviour
{
    public string screwID;          // Unique ID, set in inspector
    public ScrewType screwType;
    public bool isUnscrewed = false;

    void Start()
    {
        // Check if this screw was already unscrewed from saved state
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsScrewUnscrewed(screwID))
        {
            isUnscrewed = true;
            gameObject.SetActive(false);  // Hide it because it's already unscrewed
        }
    }

    public void TryUnscrew(Screwdriver equippedScrewdriver)
    {
        if (isUnscrewed) return;

        if (equippedScrewdriver != null && equippedScrewdriver.screwType == screwType)
        {
            Debug.Log("Correct screwdriver! Unscrewing...");
            isUnscrewed = true;
            GameStateManager.Instance.MarkScrewUnscrewed(screwID);  // Save this state
            gameObject.SetActive(false); // Hide screw after unscrewing
        }
        else
        {
            Debug.Log("Wrong screwdriver. Try a different one.");
        }
    }
}