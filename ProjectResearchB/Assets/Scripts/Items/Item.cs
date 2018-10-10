using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IActionable
{
    public readonly string itemName;
    public readonly int id;

    public Item(string itemName, int id)
    {
        this.itemName = itemName;
        this.id = id;
    }

    public abstract void Use();

    public string GetName()
    {
        return itemName;
    }
}
