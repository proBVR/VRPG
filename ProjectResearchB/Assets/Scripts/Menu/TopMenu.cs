using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenu : Menu
{
    protected override void Decide()
    {
        manager.PanelChamge(this, nextPanels[selectLine]);
    }    
}
