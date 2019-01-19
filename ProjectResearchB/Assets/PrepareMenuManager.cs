using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrepareMenuManager : MenuManager
{

    private void Start()
    {
        top.gameObject.SetActive(true);
        now = top;
    }
}
