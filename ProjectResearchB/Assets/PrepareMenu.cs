using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareMenu : Menu
{
    public StorageController storage;
    private bool flag;

    protected override void Decide(int index)
    {
        //manager.PanelChange(nextPanels[index], true);
        switch (index)
        {
            case 0:
                flag = true;
                manager.Confirm("Do you want to " + "New Game", Func);
                break;
            case 1:
                flag = false;
                manager.Confirm("Do you want to " + "Load Game", Func);
                break;
            case 2:
                manager.Confirm("Application Quit", Application.Quit);
                break;
        }
    }

    private void Func()
    {
        if (flag)
        {
            storage.Clear();
            SceneManager.LoadScene("SettingScene");
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
