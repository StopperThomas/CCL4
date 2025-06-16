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
        if (item == null) return;

        if (item.amount > 1 &&
            (item.itemType == ItemType.Cogwheel ||
             item.itemType == ItemType.Screw ||
             item.itemType == ItemType.ScrewDriver))
        {
            item.amount--;
        }
        else
        {
            itemList.Remove(item);
        }

        Debug.Log("Removed item: " + item.itemName);
        InventoryManager.Instance?.uiInventory?.RefreshInventoryItems();
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    private bool CanStack(Item a, Item b)
    {
        return a.itemType == b.itemType &&
               a.screwType == b.screwType &&
               a.screwdriverType == b.screwdriverType &&
               a.keyType == b.keyType &&
               a.cogwheelType == b.cogwheelType &&
               a.noteID == b.noteID;
    }
}
