using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MagicObject
{
    private float speed = 1;

    public override void Activate()
    {
        transform.position = user.transform.position + user.transform.forward + Vector3.up;
        var temp = user.transform.forward;
        temp.y = 0;
        transform.forward = temp;

        rb.velocity = transform.forward * speed;
    }
}
