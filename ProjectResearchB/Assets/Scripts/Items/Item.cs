using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : IActionable
{
    public readonly string callName;
    public readonly int id;

    public Item(string itemName, int id)
    {
        this.callName = itemName;
        this.id = id;
    }

    public abstract void Use();

    public string GetName()
    {
        return callName;
    }
}
