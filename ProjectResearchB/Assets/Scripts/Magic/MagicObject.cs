using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MagicObject : MonoBehaviour, IDamage
{    
    protected int power, rangeRate;
    protected AttackAttribute attribute;
    protected Character user;
    //protected Action afterInit;

    public void Init(AttackAttribute attribute, int power, int rangeRate, Character user)
    {
        this.attribute = attribute;
        this.power = power;
        this.user = user;
        this.rangeRate = rangeRate;
        Activate();
    }

    public abstract void Activate();

    public int GetPower()
    { return power; }

    public AttackAttribute GetAttribute()
    { return attribute; }

    protected void OnDestroy()
    {
        Player.instance.acting = false;
    }
}
