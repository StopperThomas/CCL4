using UnityEngine;

public class ToolPickup : MonoBehaviour
{
    public Screwdriver screwdriver;

    void OnMouseDown()
    {
        FindObjectOfType<InteractionManager>().SetEquippedScrewdriver(screwdriver);
        Debug.Log("Equipped: " + screwdriver.screwType);
        // Optionally disable the object or move it to player's hand
    }
}
