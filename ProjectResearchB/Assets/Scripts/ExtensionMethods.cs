using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void ToggleBool(this bool flag)
    {
        flag = !flag;
    }
}
