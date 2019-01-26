using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConfirmPanel : Menu
{
    private Action action;

    public void Init(string message, Action action, Menu prePanel)
    {
        this.prePanel = prePanel;
        this.action = action;
        text.text = message;
    }    

    protected override void Decide(int index)
    {
        if(index == 0) action();
        manager.PanelChange(prePanel, false);
    }
}
