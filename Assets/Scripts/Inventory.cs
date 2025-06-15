using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{

    private List<Item> itemList;

    
    public Inventory()
    {
        itemList = new List<Item>();

        Debug.Log("Inventory created");
    }
    public void AddItem(Item item)
    {
        itemList.Add(item);
        Debug.Log("Item added: " + item.itemType + " Amount: " + item.amount);
        InventoryManager.Instance.uiInventory.RefreshInventoryItems();
    }

    public void RemoveItem(Item item)
    {
        if (item.itemType == Item.ItemType.Gear)
        {
            item.amount--;
            if (item.amount <= 0)
                itemList.Remove(item);
        }
        else
        {
            itemList.Remove(item);
        }

        Debug.Log("Removed item: " + item.itemName);
        InventoryManager.Instance.uiInventory.RefreshInventoryItems();
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
    
}
