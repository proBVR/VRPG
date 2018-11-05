using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : IActionable
{
    protected string callName;
    protected readonly int modelNum, power;
    protected Arm arm;    

    private int count;
    private readonly int mergin = 2, limit = 100;
    public Vector2[] points;
    public int state;

    public Skill(string callName, int modelNum, int power)
    {
        this.callName = callName;
        this.modelNum = modelNum;
        this.power = power;
    }

    public void Use()
    {
        state = 0;
        arm.BeginSkill(PreMove);
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

    private void PreMove()
    {
        if (InArea(points[state]))
        {
            state++;
            if (state == points.Length) Activate();
            else if (state == 1) count = limit;
        }
        else if (count > 0)
        {
            count--;
            if (count == 0) state = 0;
        }
    }

    private bool InArea(Vector2 point)
    {
        var pos = arm.transform.parent.localPosition;
        if (Mathf.Abs(point.x - pos.x) < mergin && Mathf.Abs(point.y - pos.y) < mergin) return true;
        else return false;
    }
}
