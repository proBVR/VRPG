using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionObject : MonoBehaviour
{
    protected int power, range = 1;
    protected Rigidbody rb;
    AttackAttribute attribute;
    protected Character user;

    public void Init(AttackAttribute attribute, int power, int limit, Character user)
    {
        rb = GetComponent<Rigidbody>();
        this.attribute = attribute;
        this.power = power;
        this.user = user;
        //SetMove();
        Destroy(this, limit);
    }

    public abstract void Activate();

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
