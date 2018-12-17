using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MagicObject
{
    private Vector3 dir;
    private float speed = 0.01f;
    private Rigidbody rb;
	
	// Update is called once per frame
	void Update () {
        transform.position += dir * speed;
	}

    protected override void SetMove()
    {
        dir = Player.instance.GetArm(true).transform.forward;
    }

    public override Vector3 GetPos()
    {
        var arm = Player.instance.GetArm(true).transform;
        return arm.position + arm.forward;
    }

    public override Quaternion GetRot()
    {
        return Player.instance.GetArm(true).transform.rotation;
    }
}
