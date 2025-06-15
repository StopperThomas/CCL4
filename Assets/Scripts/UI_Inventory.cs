using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;


    private void Awake()
    {
        if (itemSlotContainer == null)
            itemSlotContainer = transform.Find("itemSlotContainer");

        if (itemSlotTemplate == null && itemSlotContainer != null)
            itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");

        if (itemSlotTemplate != null)
            itemSlotTemplate.gameObject.SetActive(false);
        else
            Debug.LogError(" itemSlotTemplate not found. Check hierarchy or assign manually.");
    }


    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();

    }

    public void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        List<Item> itemList = inventory.GetItemList();
        Debug.Log("🔄 Refreshing UI. Items in inventory: " + itemList.Count);

        foreach (Item item in itemList)
        {
            Debug.Log("🧩 Adding item: " + item.itemName + ", amount: " + item.amount);

            Transform itemSlot = Instantiate(itemSlotTemplate, itemSlotContainer);
            itemSlot.gameObject.SetActive(true);

            Image image = itemSlot.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            

            // Add click listener to inspect
            Button button = itemSlot.GetComponent<Button>();
            button.onClick.RemoveAllListeners(); // Prevent duplicates
            button.onClick.AddListener(() =>
            {
                FindObjectOfType<ItemInspectorUI>()?.ShowItem(item);
            });
        }
    }
}
