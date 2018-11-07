﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : IActionable
{
    public static readonly Vector2[] points =
        { new Vector2(-5,  5), new Vector2( 0,  5), new Vector2( 5,  5),
          new Vector2(-5,  0), new Vector2( 0,  0), new Vector2( 5,  0),
          new Vector2(-5, -5), new Vector2( 0, -5), new Vector2( 5, -5) };

    protected string callName;
    protected readonly int modelNum, power;
    protected Arm arm;    

    private int count;
    private readonly int mergin = 2, limit = 100, time = 10;
    public int state;
    public int[] moveList;

    /*
    move: 1-9
      1 2 3
      4 5 6
      7 8 9
    */

    //moveは動かす順番の逆順、0を含んではならない
    public Skill(string callName, int modelNum, int power, int move)
    {
        this.callName = callName;
        this.modelNum = modelNum;
        this.power = power;

        List<int> temp = new List<int>();
        while(move != 0)
        {
            temp.Add(move % 10 - 1);
            move /= 10;
        }
        moveList = temp.ToArray();
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
        prefab.Init(AttackAttribute.normal, power, time);
    }

    public string GetName()
    {
        return callName;
    }

    private void PreMove()
    {
        if (InArea(moveList[state]))
        {
            state++;
            if (state == moveList.Length) Activate();
            else if (state == 1) count = limit;
        }
        else if (count > 0)
        {
            count--;
            if (count == 0) state = 0;
        }
    }

    private bool InArea(int index)
    {
        var point = points[index];
        var pos = arm.transform.parent.localPosition;
        if (Mathf.Abs(point.x - pos.x) < mergin && Mathf.Abs(point.y - pos.y) < mergin) return true;
        else return false;
    }
}
