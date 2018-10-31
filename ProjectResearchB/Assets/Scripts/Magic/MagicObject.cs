using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicObject : ActionObject
{
    private int power, range=1;
    AttackAttribute attribute;

    public void Init(AttackAttribute attribute, int power)
    {
        this.attribute = attribute;
        this.power = power;
        SetMove();
    }

    protected abstract void SetMove();

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().GetStatus().Damage(power);
        }
    }
}
