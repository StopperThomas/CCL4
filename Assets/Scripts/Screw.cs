using UnityEngine;

public class Screw : MonoBehaviour
{
    public ScrewType screwType;
    public bool isUnscrewed = false;

    public void TryUnscrew(Screwdriver equippedScrewdriver)
    {
        if (isUnscrewed) return;

        if (equippedScrewdriver != null && equippedScrewdriver.screwType == screwType)
        {
            Debug.Log("Correct screwdriver! Unscrewing...");
            isUnscrewed = true;
            // Add unscrew animation, sound, or hide the screw
            gameObject.SetActive(false); // Example behavior
        }
        else
        {
            Debug.Log("Wrong screwdriver. Try a different one.");
            // Optionally play a failure sound or UI feedback
        }
    }
}
