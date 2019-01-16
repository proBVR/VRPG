using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class NewGamePanel : Menu
{
    public StorageController storage;

    protected override void Decide(int index)
    {
        switch (index)
        {
            case 0://NweGame
                storage.Clear();
                SceneManager.LoadScene("SettingScene");
                break;
            default:
                manager.PanelChange(prePanel, false);
                break;
        }
    }
}
