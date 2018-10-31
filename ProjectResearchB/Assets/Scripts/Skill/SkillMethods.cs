using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillMethods
{
    public static Func<bool>[] preMoves = { SampleMove };

    private static int limit, count;
    private static readonly int mergin = 2;
    public static Vector2[] points;
    public static int state;

    private static bool SampleMove()
    {

        if (InArea(points[state]))
        {
            state++;
            if (state == points.Length) return true;
            if (state == 1) count = limit;
        }
        else if (count > 0)
        {
            count--;
            if (count == 0) state = 0;
        }
        return false;
    }

    private static bool InArea(Vector2 point)
    {
        var pos = Player.instance.GetArm(true).transform.localPosition;
        if (Mathf.Abs(point.x-pos.x) < mergin && Mathf.Abs(point.y-pos.y) < mergin) return true; 
        else return false;
    }
}
