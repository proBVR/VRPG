using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Magic : MonoBehaviour, IActionable
{
    protected string callName;

    public abstract void Use();

    public  string GetName()
    {
        return callName;
    }
}
