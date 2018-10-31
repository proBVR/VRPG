using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : IActionable
{
    protected string callName;
    protected readonly int interval, modelNum, power;
    protected readonly GameObject skillObject;
    protected Arm arm;
    protected Func<bool> preMove;
    public readonly Vector2[] points;

    public Skill(string callName, int modelNum, int power)
    {
        this.callName = callName;
        this.modelNum = modelNum;
        this.power = power;
    }

    public void Use()
    {
        arm.BeginSkill(preMove, Activate);
    }

    protected void Activate()
    {
        arm.FinSkill();
        var prefab = MyGameManager.instance.GenSkill(modelNum);
        prefab.Init();
    }

    public string GetName()
    {
        return callName;
    }
}
