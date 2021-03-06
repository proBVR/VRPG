﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill : IActionable
{
    private enum Position
    {
        LeftUp, MiddleUp, RightUp,
        LeftMiddle, MiddleMiddle, RightMiddle,
        LeftDown, MiddleDown, RightDown
    }

    public static readonly Vector2[] points =
        { new Vector2(-0.25f,  1.55f), new Vector2( 0.15f,  1.55f), new Vector2( 0.55f,  1.55f),
          new Vector2(-0.25f,  1.2f ), new Vector2( 0.15f,  1.2f ), new Vector2( 0.55f,  1.2f ),
          new Vector2(-0.25f,  0.85f), new Vector2( 0.15f,  0.85f), new Vector2( 0.55f,  0.85f) };

    [SerializeField]
    private string name;
    [SerializeField]
    private int modelNum, power, cost;
    //[SerializeField]
    private bool hand = true;
    private Arm arm;

    private int count;
    private readonly int limit = 300;
    private readonly float mergin = 0.15f;
    private int state;
    [SerializeField]
    private Position[] moveList;
    private SkillObject entity;


    /*
    move: 1-9
      1 2 3
      4 5 6
      7 8 9
    */

    //SlillもとりあえずPCのみで
    public void Use(Character user)
    {
        Debug.Log("use skill: " + name);
        state = 0;
        arm = Player.instance.GetArm(hand);
        arm.BeginSkill(PreMove);
        entity = GameManager.instance.GenSkill(modelNum);
        entity.Init(AttackAttribute.normal, power, user);
    }

    public string GetName()
    {
        return name;
    }

    private void PreMove()
    {
        if (InArea((int)moveList[state]))
        {
            state++;
            SoundController.decide_sound.PlayOneShot(SoundController.decide_sound.clip);
            Debug.Log("state: " + state);
            if (state == moveList.Length)
            {
                entity.Activate();
                arm.FinSkill();
            }
            else if (state == 1) count = limit;
        }
        else if (count > 0)
        {
            count--;
            if (count == 0)
            {
                state = 0;
                entity.Reset();
                //タイムアウトー＞ミス音
            }
        }
    }

    private bool InArea(int index)
    {
        var point = points[index];
        var pos = arm.transform.parent.position - Player.instance.transform.position;
        pos = Quaternion.AngleAxis(-Player.instance.transform.eulerAngles.y, Vector3.up) * pos;
        //Debug.Log("pos: "+pos.x+", "+pos.y);
        if (Mathf.Abs(point.x - pos.x) < mergin && Mathf.Abs(point.y - pos.y) < mergin) return true;
        else return false;
    }

    public int GetCost()
    {
        return cost;
    }
}
