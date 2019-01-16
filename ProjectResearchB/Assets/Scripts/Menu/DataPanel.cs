using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataPanel : Menu
{
    public StorageController storage;

    protected override void Decide(int index)
    {
        switch (index)
        {
            case 0://Save
                storage.Save();
                break;
            case 1://Load
                storage.Load();
                break;
            default:
                break;
        }
    }
}
