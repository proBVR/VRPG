using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class DataPanel : Menu
{
    public StorageController storage;

    protected override void Decide(int index)
    {
        switch (index)
        {
            case 0://Save
                manager.Confirm("DO: Save", storage.Save);
                break;
            case 1://Load
                manager.Confirm("DO: Load", storage.Load);
                break;
            case 2:
                manager.Confirm("Return: Prepare Scene", ReturnScene);
                break;
            default:
                break;
        }
    }

    private void ReturnScene()
    {
        SceneManager.LoadScene("PrepareScene");
    }
}
