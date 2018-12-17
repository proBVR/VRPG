using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemMethods
{
    public static readonly Action<int>[] uses = {HpRecover, MpRecover, FullRecover};

    private static void HpRecover(int value)
    {
        Player.instance.GetStatus().RecoverHp(value);
    }

	private static void MpRecover(int value)
    {
        Player.instance.GetStatus().Mp = value;
    }

    private static void FullRecover(int value)
    {

    }
}
