using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionObject : MonoBehaviour
{
    public abstract Vector3 GetPos();
    public abstract Quaternion GetRot();

    private int power, range = 1;
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
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().GetStatus().Damage(power);
        }
    }

    protected void Extinction()
    {
        Player.instance.acting = false;
        Destroy(this);
    }
}
