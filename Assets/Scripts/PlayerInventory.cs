using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    
    // This is a very unpolished class. Later we could change it to be an Item --> int mapping.
    // (where items have a description, sprite, etc...)
    public readonly Dictionary<string, int> _items = new Dictionary<string, int>();

    public void addItem(string itemName)
    {
        if (_items.ContainsKey(itemName))
        {
            _items[itemName] += 1;
        }
        else
        {
            _items[itemName] = 1;
        }
    }

    public bool hasItem(string itemName)
    {
        if (_items.ContainsKey(itemName))
        {
            if (_items[itemName] > 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool removeItem(string itemName)
    {
        if (hasItem(itemName))
        {
            _items[itemName] -= 1;
            return true;
        }
        return false;
    }
}
