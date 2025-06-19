using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    if (inventory == null)
    {
        Debug.LogError("UI_Inventory: inventory is null! Did you forget to call SetInventory()?");
        return;
    }

    foreach (Transform child in itemSlotContainer)
    {
        if (child == itemSlotTemplate) continue;
        Destroy(child.gameObject);
    }

    List<Item> itemList = inventory.GetItemList();
    int maxSlots = 20;

    for (int i = 0; i < maxSlots; i++)
    {
        Transform itemSlot = Instantiate(itemSlotTemplate, itemSlotContainer);
        itemSlot.gameObject.SetActive(true);

        Image image = itemSlot.Find("image").GetComponent<Image>();
        TextMeshProUGUI amountText = itemSlot.Find("amount").GetComponent<TextMeshProUGUI>();
        Button button = itemSlot.GetComponent<Button>();

        if (i < itemList.Count)
        {
            Item item = itemList[i];
            image.sprite = item.GetSprite();
            image.enabled = true;
            amountText.text = item.amount > 1 ? item.amount.ToString() : "";

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                selectedItem = item;
                FindObjectOfType<ItemInspectorUI>()?.ShowItem(item);
            });
        }
        else
        {
            image.sprite = null;
            image.enabled = false;
            amountText.text = "";
            button.onClick.RemoveAllListeners(); // empty slot
        }
    }

    Debug.Log($"UI_Inventory: Refreshed {Mathf.Min(itemList.Count, maxSlots)} item slots.");
}


    public void UpdateEquippedSlot(Item item)
    {
        if (equippedSlotIcon == null || equippedSlotObject == null) return;

        if (item != null)
        {
            equippedSlotIcon.sprite = item.GetSprite();
            equippedSlotIcon.enabled = true;
            equippedSlotObject.SetActive(true);
        }
        else
        {
            equippedSlotIcon.sprite = null;
            equippedSlotIcon.enabled = false;
            equippedSlotObject.SetActive(false);
        }
    }

    public void TryEquipItem(Item item)
    {
        if (IsEquipable(item))
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

 private bool IsEquipable(Item item)
{
    return item != null && (
        item.itemType == ItemType.ScrewDriver ||
        item.itemType == ItemType.Key ||
        item.itemType == ItemType.Screw ||
        item.itemType == ItemType.Cogwheel ||
        item.itemType == ItemType.LightBulb
    );
}


    public Item GetSelectedItem()
    {
        return selectedItem;
    }
}
