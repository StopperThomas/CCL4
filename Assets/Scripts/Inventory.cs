using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
        Debug.Log("Inventory created");
    }

    public void AddItem(Item newItem)
    {
        if (itemList.Count >= 20)
        {
            Debug.LogWarning("Inventory full: Max 20 items.");
            return;
        }

        bool itemStacked = false;
        foreach (Item existingItem in itemList)
        {
            if (CanStack(existingItem, newItem))
            {
                existingItem.amount += newItem.amount;
                itemStacked = true;
                break;
            }
        }

        if (!itemStacked)
        {
            itemList.Add(newItem);
        }

        InventoryManager.Instance?.uiInventory?.RefreshInventoryItems();
    }


    public void RemoveItem(Item item)
    {
        Item inventoryItem = null;

        if (CanStackable(item.itemType))
        {
            inventoryItem = itemList.Find(i => CanStack(i, item));
        }
        else
        {
            // For unstackables like keys, find exact match
            inventoryItem = itemList.Find(i => i == item);
        }

        if (inventoryItem == null)
        {
            Debug.LogWarning("Item not found in inventory.");
            return;
        }

        if (inventoryItem.amount > 1)
        {
            inventoryItem.amount--;
        }
        else
        {
            itemList.Remove(inventoryItem);
        }

        Debug.Log("Removed item: " + inventoryItem.itemName);
        InventoryManager.Instance?.uiInventory?.RefreshInventoryItems();
    }

    private bool CanStackable(ItemType type)
    {
        return type == ItemType.Screw ||
               type == ItemType.ScrewDriver ||
               type == ItemType.Cogwheel ||
               type == ItemType.LightBulb;
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    private bool CanStack(Item a, Item b)
    {
        if (a.itemType != b.itemType)
            return false;

        switch (a.itemType)
        {
            case ItemType.Screw:
                return a.screwType == b.screwType;
            case ItemType.ScrewDriver:
                return a.screwdriverType == b.screwdriverType;
            case ItemType.Cogwheel:
                return a.cogwheelType == b.cogwheelType;
            case ItemType.LightBulb:
                return true; // Light bulbs can always stack
            case ItemType.Key:
                return false;
            default:
                return false;
        }
    }

}
