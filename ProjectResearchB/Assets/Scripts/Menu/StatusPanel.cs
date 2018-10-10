using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : Menu
{
    protected override void SetData()
    {
        var player = Player.instance;
        text.text = "HP: " + player.GetStatus("hp") + "/" + player.GetStatus("maxHp") + "\n"
            + "MP: " + player.GetStatus("mp") + "/" + player.GetStatus("maxMp") + "\n"
            + "STR: " + player.GetStatus("str") + "\n" + "VIT: " + player.GetStatus("vit") + "\n" + "SPEED: " + player.GetStatus("speed");
    }
}
