using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadGamePanel : Menu
{
    public StorageController storage;

    protected override void Decide(int index)
    {
        switch (index)
        {
            case 0://NweGame
                storage.Load();
                break;
            default:
                manager.PanelChange(prePanel, false);
                break;
        }
    }
}
