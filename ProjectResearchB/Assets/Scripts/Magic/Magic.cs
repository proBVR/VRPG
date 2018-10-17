using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Magic : IActionable
{
    protected string callName;
    protected int rank, attribute, modelNum, power;

    public Magic(string callName, int rank, int attribute, int modelNum, int power)
    {
        this.callName = callName;
        this.rank = rank;
        this.attribute = attribute;
        this.modelNum = modelNum;
        this.power = power;
    }

    public void Use()
    {
        var prefab = MyGameManager.instance.GenMagic(modelNum);
        prefab.Init(attribute, power);
    }

    public  string GetName()
    {
        return callName;
    }
}
