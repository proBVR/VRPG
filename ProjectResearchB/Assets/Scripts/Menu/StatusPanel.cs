using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : Menu
{
    protected override void SetData()
    {
        text.text = Player.instance.GetStatus().ToString();
    }
}
