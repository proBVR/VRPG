﻿using System.Collections;
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
            case 0://Reset
                uc.Reset();
                break;
            default:
                break;
        }
    }
}
