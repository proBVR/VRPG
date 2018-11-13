using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    private Menu now;
    [SerializeField]
    private Menu top;
    [SerializeField]
    private ConfirmPanel cm;

    public void PanelChange(Menu to, bool flag)
    {
        if (now != null) now.gameObject.SetActive(false);
        now = to;
        to.gameObject.SetActive(true);
        if(flag) to.Reset();        
    }

    public void MenuReset()
    {
        PanelChange(top, true);
    }

    public void Confirm(string message, Action action)
    {
        cm.gameObject.SetActive(true);
        cm.Init(message, action, now);
        PanelChange(cm, false);
    }
}
