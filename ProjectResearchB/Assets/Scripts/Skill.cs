using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour, IActionable
{
    protected string callName;
    protected readonly int interval;
    protected int counter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 0) counter--;
    }

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
