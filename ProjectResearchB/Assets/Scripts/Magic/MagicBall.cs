using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MagicObject
{
    private float speed = 1;

    public override void Activate()
    {
        var arm = Player.instance.GetArm(true).transform;
        transform.position = arm.position + arm.forward;
        transform.rotation = arm.rotation;
        rb.velocity = transform.forward * speed;
    }
}
