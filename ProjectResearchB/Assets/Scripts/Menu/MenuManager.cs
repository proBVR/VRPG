using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    protected Menu now;
    [SerializeField]
    protected Menu top;
    [SerializeField]
    private ConfirmPanel cm;

    public void PanelChange(Menu to, bool flag)
    {
        if (now != null) now.gameObject.SetActive(false);
        now = to;
        to.gameObject.SetActive(true);
        Debug.Log("panel change");
        if(flag) to.ResetData();
    }

    public void MenuReset()
    {
        Debug.Log("menu reset");
        PanelChange(top, true);
    }

    public void Confirm(string message, Action action)
    {
        cm.gameObject.SetActive(true);
        cm.Init(message, action, now);
        PanelChange(cm, false);
    }
}
