using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionObject : MonoBehaviour
{
    public abstract Vector3 GetPos();
    public abstract Quaternion GetRot();

    protected int power, range = 1;
    protected Rigidbody rb;
    AttackAttribute attribute;

    public void Init(AttackAttribute attribute, int power, int limit)
    {
        rb = GetComponent<Rigidbody>();
        this.attribute = attribute;
        this.power = power;
        SetMove();
        Destroy(this, limit);
    }

    protected abstract void SetMove();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().GetStatus().Damage(power, attribute);
        }
    }

    protected void OnDestroy()
    {
        Player.instance.acting = false;
    }
}
