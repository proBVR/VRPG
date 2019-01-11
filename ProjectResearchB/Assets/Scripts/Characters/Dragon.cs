using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Enemy
{
    private float mvSpeed = 2;
    private bool attacking = false;
    [SerializeField]
    private AtkObject[] atkObjects;

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
        if (attacking) return;
        attacking = true;
        Scheduler.instance.AddEvent(3, FinAtk);
        //3 pattern
        switch (index)
        {
            case 0://ひっかき（前方）
                atkObjects[index].AtKBegin(status.Str, AttackAttribute.normal, 0.5f, 2);
                break;
            case 1://ジャンプ（周囲）
                atkObjects[index].AtKBegin(status.Str, AttackAttribute.normal, 1.5f, 2);
                break;
            case 2://火炎放射
                atkObjects[index].AtKBegin(status.Str, AttackAttribute.fire, 1, 2.5f);
                break;
        }
        actNum++;
    }

    private void FinAtk()
    {
        attacking = false;
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
