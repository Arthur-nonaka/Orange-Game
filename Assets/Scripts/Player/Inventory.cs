using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSize = 1;
    public Dictionary<Item, int> items = new Dictionary<Item, int>();

    public void AddItem(Item item)
    {
        if (items.Count >= maxSize && !items.ContainsKey(item))
        {
            items[item]++;
        }
        else
        {
            items[item] = 1;
        }
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
    }

    public void RemoveKey(Item item)
    {
        if (items.ContainsKey(item))
        {
            items.Remove(item);
        }
    }
}
