using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareMenu : Menu
{
    protected override void Decide(int index)
    {
        manager.PanelChange(nextPanels[index], true);
    }
}
