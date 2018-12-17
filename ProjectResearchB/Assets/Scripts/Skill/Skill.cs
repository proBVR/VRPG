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
    private int modelNum, power;
    private Arm arm;    

    private int count;
    private readonly int limit = 100, time = 10;
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

    ////moveは動かす順番の逆順、0を含んではならない
    //public Skill(string callName, int modelNum, int power, int move)
    //{
    //    arm = Player.instance.GetArm(true);
    //    this.name = callName;
    //    this.modelNum = modelNum;
    //    this.power = power;

    //    List<int> temp = new List<int>();
    //    while(move != 0)
    //    {
    //        temp.Add(move % 10 - 1);
    //        move /= 10;
    //    }
    //    moveList = temp.ToArray();
    //}

    public void Use()
    {
        state = 0;
        arm.BeginSkill(PreMove);
        entity = GameManager.instance.GenSkill(modelNum);
        entity.Init(AttackAttribute.normal, power, time);
    }

    protected void Activate()
    {
        arm.FinSkill();
        
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
        var pos = arm.transform.parent.position - Player.instance.transform.position;
        //Debug.Log("pos: "+pos.x+", "+pos.y);
        if (Mathf.Abs(point.x - pos.x) < mergin && Mathf.Abs(point.y - pos.y) < mergin) return true;
        else return false;
    }
}
