using UnityEngine;

public class ToolPickup : MonoBehaviour
{
    public Screwdriver screwdriver;

    public void EquipTool()
    {
        FindObjectOfType<InteractionManager>().SetEquippedScrewdriver(screwdriver);
        Debug.Log("Equipped: " + screwdriver.screwType);
    }
}
