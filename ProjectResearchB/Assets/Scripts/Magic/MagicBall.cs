using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MagicObject
{
    [SerializeField]
    protected float range;

    private float speed = 1;
    private Rigidbody rb;

    public override void Activate()
    {
        range *= (1 + 0.5f * (rangeRate - 1));
        Destroy(this, range / speed);

        transform.position = user.transform.position + user.transform.forward + Vector3.up;
        var temp = user.transform.forward;
        temp.y = 0;
        transform.forward = temp;

        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ((user.IsPlayer) ? "Enemy" : "Player"))
        {
            other.GetComponent<Character>().GetStatus().Damage(this);
        }
    }
}
