using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Enemy
{
    //private float mvSpeed = 2;
    //private bool attacking = false;
    [SerializeField]
    private AtkObject[] atkObjects;

    private bool stop = false;

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
        //if (attacking) return;
        attacking = true;
        Scheduler.instance.AddEvent(3, FinAtk);
        //3 pattern
        switch (index)
        {
            case 0://ひっかき（前方）
                atkObjects[index].AtKBegin(status.Str, AttackAttribute.normal, 0.5f, 2);
                animator.SetInteger("attacking", 1);
                break;
            case 1://ジャンプ（周囲）
                atkObjects[index].AtKBegin(status.Str, AttackAttribute.normal, 1.5f, 2);
                animator.SetInteger("attacking", 2);
                break;
            case 2://火炎放射
                atkObjects[index].AtKBegin(status.Str, AttackAttribute.fire, 1, 2.5f);
                animator.SetInteger("attacking", 3);
                break;
        }
        actNum = (actNum + 1) % 3;
    }

    protected override void Idle()
    {
        if (stop) return;
        var pos = transform.localPosition;
        if (Mathf.Abs(pos.x) < 0.2 && Mathf.Abs(pos.z) < 0.2)
        {
            animator.SetBool("running", false);
            stop = true;
            transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            //move to start position
            transform.LookAt(manager.transform.position);
            transform.forward -= new Vector3(0, transform.forward.y, 0);
            rb.velocity = transform.forward * mvSpeed;
            animator.SetBool("running", true);
        }
    }
}
