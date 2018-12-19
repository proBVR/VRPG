using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicObject : ActionObject
{
    protected void Start()
    {
        Activate();
    }
}
