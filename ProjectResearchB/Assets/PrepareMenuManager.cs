using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrepareMenuManager : MonoBehaviour
{
    private Menu now;
    [SerializeField]
    private Menu top;
    [SerializeField]
    private ConfirmPanel cm;

    private void Start()
    {
        top.gameObject.SetActive(true);
    }

    public void PanelChange(Menu to, bool flag)
    {
        if (now != null) now.gameObject.SetActive(false);
        now = to;
        to.gameObject.SetActive(true);
        Debug.Log("panel change");
        if (flag) to.Reset();
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
