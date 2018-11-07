using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenu : Menu
{
    protected override void Decide(int index)
    {
        manager.PanelChamge(this, nextPanels[index]);
    }    
}
