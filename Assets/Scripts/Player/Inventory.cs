using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSize = 1;
    public Dictionary<Item, int> items = new Dictionary<Item, int>();

    public delegate void InventoryChangedDelegate(float newAmount);
    public event InventoryChangedDelegate OnInventoryChanged;

    public void AddItem(Item item)
    {
        if (GetTotalItemCount() >= maxSize && !items.ContainsKey(item))
        {
            return;
        }

        if (items.ContainsKey(item))
        {
            items[item]++;
        }
        else
        {
            items[item] = 1;
        }
        OnInventoryChanged?.Invoke(GetTotalItemCount());
    }

    public int GetTotalItemCount()
    {
        int total = 0;
        foreach (var count in items.Values)
        {
            total += count;
        }

        return total;
    }

    public int GetItemCount(Item item)
    {
        return items.ContainsKey(item) ? items[item] : 0;
    }

    public Dictionary<Item, int> GetAllItems()
    {
        return items;
    }

    public void RemoveItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item]--;
            if (items[item] <= 0)
            {
                items.Remove(item);
            }
        }
        OnInventoryChanged?.Invoke(GetTotalItemCount());
    }

    public void RemoveKey(Item item)
    {
        if (items.ContainsKey(item))
        {
            items.Remove(item);
        }
        OnInventoryChanged?.Invoke(GetTotalItemCount());
    }
}
