using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShot : SkillObject
{
    private Vector3 dir;
    private int speed = 1;
	
	// Update is called once per frame
	void Update () {
        transform.position += dir * speed;
	}

    protected override void SetMove()
    {
        dir = Player.instance.transform.eulerAngles;
    }

    public override Vector3 GetPos()
    {
        return Player.instance.transform.position + Player.instance.transform.forward;
    }

    public override Quaternion GetRot()
    {
        return Player.instance.transform.rotation;        
    }
}