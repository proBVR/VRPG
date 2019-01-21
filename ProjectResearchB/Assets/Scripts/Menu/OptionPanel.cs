using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanel : Menu
{
    [SerializeField]
    private UserCamera uc;
    protected override void Decide(int index)
    {
        switch (index)
        {
            case 0://高さリセット
                manager.Confirm("Do: Height Reset\n", uc.HeightReset);
                break;
            case 1://スケール調整
                manager.Confirm("Do: Width Reset\n", uc.WidthReset);
                break;
            case 2:
                manager.Confirm("Do: Rot Reset", Player.instance.ResetRot);
                break;
            default:
                break;
        }
    }
}
