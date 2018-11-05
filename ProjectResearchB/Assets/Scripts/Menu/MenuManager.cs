using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private Menu now;
    [SerializeField]
    private Menu top;

    public void PanelChamge(Menu from, Menu to)
    {
        now = to;
        to.gameObject.SetActive(true);
        from.gameObject.SetActive(false);
    }

    public void MenuReset()
    {
        PanelChamge(now, top);
    }
}
