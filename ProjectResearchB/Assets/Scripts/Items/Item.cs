using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : IActionable
{
    private enum Method
    {
        HpRecover,
        MpRecover,
        FullRecover
    }


    [SerializeField]
    private string name;
    [SerializeField]
    private int value;
    [SerializeField]
    private Method method;

    public void Use()
    {
        if (Player.instance.inventory.IsInclude(name))
        {
            Player.instance.inventory.DecInventory(name);
            ItemMethods.uses[(int)method](value);
        }
        Player.instance.acting = false;
    }

    public string GetName()
    {
        return name;
    }
}
