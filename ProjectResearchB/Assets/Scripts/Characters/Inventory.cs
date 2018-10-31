using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private readonly int max = 9;
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public void AddInventory(string name)
    {
        if (inventory.ContainsKey(name))
        {
            inventory[name] += 1;
            if (inventory[name] > max) inventory[name] = max;
        }
        else inventory[name] = 1;
    }

    public void DecInventory(string name)
    {
        if (inventory.ContainsKey(name))
        {
            inventory[name]--;
            if (inventory[name] == 0) inventory.Remove(name);
        }        
    }

    public bool IsInclude(string name)
    {
        return inventory.ContainsKey(name);
    }
	
}
