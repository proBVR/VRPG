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
        if (index == 0) flag = true;
        else flag = false;
        manager.Confirm("Do you want to " + ((flag) ? "New Game":"Load Game"), Func);
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
