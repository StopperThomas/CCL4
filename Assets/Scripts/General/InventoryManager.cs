using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public Inventory inventory;
    public UI_Inventory uiInventory;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (inventory == null)
            {
                Debug.LogWarning("Inventory was null. Creating new one.");
                inventory = new Inventory(); 
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}