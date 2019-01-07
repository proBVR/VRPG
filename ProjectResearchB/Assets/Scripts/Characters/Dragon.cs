using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Enemy
{
    private float mvSpeed = 2;

    protected override void Move()
    {
        //move to player
        transform.LookAt(Player.instance.transform.position);
        transform.forward -= new Vector3(0, transform.forward.y, 0);
        rb.velocity = transform.forward * mvSpeed;
        animator.SetBool("running", true);
    }

    protected override void Action(int index)
    {
        //3 pattern
        switch (index)
        {
            case 0://ひっかき（前方）
                break;
            case 1://ジャンプ（周囲）
                break;
            case 2://火炎放射
                break;
        }
    }

    protected override void Idle()
    {
        //move to start position
        transform.LookAt(manager.transform.position);
        transform.forward -= new Vector3(0, transform.forward.y, 0);
        rb.velocity = transform.forward * mvSpeed;
        animator.SetBool("running", true);
    }
}
