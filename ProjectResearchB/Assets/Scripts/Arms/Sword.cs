using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Arm, IDamage
{
    public int GetPower()
    {
        return attack + Player.instance.GetStatus().Str;
    }

    public AttackAttribute GetAttribute()
    {
        return AttackAttribute.normal;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!cooling && other.gameObject.tag == "Enemy")
        {
            cooling = true;
            Scheduler.AddEvent(FinCooling, interval);
            other.GetComponent<Character>().GetStatus().Damage(this);
            //Debug.Log("hit: te, left: " + other.GetComponent<Enemy>().GetHp());
        }
    }

}
