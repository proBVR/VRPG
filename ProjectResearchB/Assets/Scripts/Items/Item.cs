using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : IActionable
{
    public readonly string callName;
    public readonly int id, value;
    private Action<int> use;

    public Item(string callName, int id, int value, Action<int> use)
    {
        this.use = use;
        this.callName = callName;
        this.id = id;
        this.value = value;
    }

    public void Use()
    {
        if (Player.instance.inventory.IsInclude(callName))
        {
            Player.instance.inventory.DecInventory(callName);
            use(value);
        }
        Player.instance.acting = false;
    }

    public string GetName()
    {
        return callName;
    }
}
