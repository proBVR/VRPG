using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewGamePanel : Menu
{
    public StorageController storage;

    protected override void Decide(int index)
    {
        switch (index)
        {
            case 0://NweGame
                storage.Clear();
                break;
            default:
                manager.PanelChange(prePanel, false);
                break;
        }
    }
}
