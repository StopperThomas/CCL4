using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;

    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;

    [Header("Equipped Item UI")]
    [SerializeField] private GameObject equippedSlotObject;
    [SerializeField] private Image equippedSlotIcon;

    private Item selectedItem;

    private void Awake()
    {
        if (itemSlotContainer == null)
            itemSlotContainer = transform.Find("itemSlotContainer");

        if (itemSlotTemplate == null && itemSlotContainer != null)
            itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");

        if (itemSlotTemplate != null)
            itemSlotTemplate.gameObject.SetActive(false);
        else
            Debug.LogError("itemSlotTemplate not found. Check hierarchy or assign manually.");
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
        Debug.Log("Refreshing UI. Items in inventory: " + itemList.Count);

        foreach (Item item in itemList)
        {
            Debug.Log("Adding item: " + item.itemName + ", amount: " + item.amount);

            Transform itemSlot = Instantiate(itemSlotTemplate, itemSlotContainer);
            itemSlot.gameObject.SetActive(true);

            Image image = itemSlot.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            // Add click listener to inspect
            Button button = itemSlot.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                selectedItem = item;
                FindObjectOfType<ItemInspectorUI>()?.ShowItem(item);
            });
        }
    }

   public void UpdateEquippedSlot(Item item)
{
    if (equippedSlotIcon == null || equippedSlotObject == null) return;

    if (item != null)
    {
        equippedSlotIcon.sprite = item.GetSprite();
        equippedSlotIcon.enabled = true;
        equippedSlotObject.SetActive(true); // Show the slot container
    }
    else
    {
        equippedSlotIcon.sprite = null;
        equippedSlotIcon.enabled = false;
        equippedSlotObject.SetActive(false); // Hide the slot container
    }
}


    public void TryEquipItem(Item item)
    {
        if (item.itemType == Item.ItemType.ScrewDriver)
        {
            var manager = FindObjectOfType<InteractionManager>();
            if (manager != null)
                manager.EquipFromUI(item);

            UpdateEquippedSlot(item);
            Debug.Log("Equipped: " + item.itemName);
        }
        else
        {
            Debug.Log("Can't equip item of type: " + item.itemType);
        }
    }

    public Item GetSelectedItem()
    {
        return selectedItem;
    }
}
