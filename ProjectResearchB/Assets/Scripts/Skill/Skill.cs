using System;
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
        { new Vector2(-0.5f,  2.5f), new Vector2( 0.15f,  2.5f), new Vector2( 0.8f,  2.5f),
          new Vector2(-0.5f,  1.5f), new Vector2( 0.15f,  1.5f), new Vector2( 0.8f,  1.5f),
          new Vector2(-0.5f,  0.5f), new Vector2( 0.15f,  0.5f), new Vector2( 0.8f,  0.5f) };

    [SerializeField]
    private string name;
    [SerializeField]
    private int modelNum, power, cost;
    //[SerializeField]
    private bool hand = true;
    private Arm arm;

    private int count;
    private readonly int limit = 100;
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
            }
        }
    }

    private bool InArea(int index)
    {
        var point = points[index];
        var pos = arm.transform.parent.position - Player.instance.transform.position;
        //Debug.Log("pos: "+pos.x+", "+pos.y);
        if (Mathf.Abs(point.x - pos.x) < mergin && Mathf.Abs(point.y - pos.y) < mergin) return true;
        else return false;
    }

    public int GetCost()
    {
        return cost;
    }
}
