using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : IActionable
{
    protected string callName;
    protected readonly int interval;
    protected int counter;    

    public void Use()
    {
        if (counter != 0) return;

        //skill

        counter = interval;
    }

    public string GetName()
    {
        return callName;
    }
}
