using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObject : MonoBehaviour, IDamage
{

    protected int power;
    protected AttackAttribute attribute;
    protected Character user;

    public void Init(AttackAttribute attribute, int power, Character user)
    {
        this.attribute = attribute;
        this.power = power;
        this.user = user;
    }

    public abstract void Activate();

    //予備動作失敗時の動作
    public abstract void Reset();

    public int GetPower()
    { return power; }

    public AttackAttribute GetAttribute()
    { return attribute; }

    protected void OnDestroy()
    {
        Player.instance.acting = false;
    }
}
