﻿using System;
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

    //とりあえずPC以外がアイテムを使うことは想定していない
    public void Use(Character user)
    {
        if (Player.instance.inventory.IsInclude(name))
        {
            Debug.Log("use item: " + name);
            Player.instance.inventory.DecInventory(name);
            ItemMethods.uses[(int)method](value);
        }
        Player.instance.acting = false;
    }

    public string GetName()
    {
        return name;
    }

    public int GetCost()
    {
        return -1;
    }
}
