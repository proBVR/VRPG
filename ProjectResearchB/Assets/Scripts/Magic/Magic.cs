using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : IActionable
{
    protected string callName;
    protected int rank, modelNum, power, time = 10;
    protected AttackAttribute attribute;

    public Magic(string callName, int rank, AttackAttribute attribute, int modelNum, int power)
    {
        this.callName = callName;
        this.rank = rank;
        this.attribute = attribute;
        this.modelNum = modelNum;
        this.power = power;
    }

    public void Use()
    {
        var prefab = GameManager.instance.GenMagic(modelNum);
        prefab.Init(attribute, power, time);
    }

    public  string GetName()
    {
        return callName;
    }
}