using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    //private float mvSpeed = 2;
    //private bool attacking = false;
    [SerializeField]
    private AtkObject[] atkObject;

    protected override void Move()
    {
        transform.LookAt(Player.instance.transform.position);
        transform.forward -= new Vector3(0, transform.forward.y, 0);
        rb.velocity = transform.forward * mvSpeed;
        animator.SetBool("running", true);
    }

    protected override void Action(int index)
    {
        Scheduler.AddEvent(FinAtk, 2);
        atkObject[0].AtKBegin(status.Str, AttackAttribute.normal, 0.5f, 1.5f);
        atkObject[1].AtKBegin(status.Str, AttackAttribute.normal, 0.5f, 1.5f);
    }

    protected override void Idle()
    {
        animator.SetBool("running", false);
        rb.velocity = Vector3.zero;
    }

    protected override void Stop()
    {
        animator.SetBool("running", false);
        rb.velocity = Vector3.zero;
    }

    protected override void Death()
    {
        base.Death();
        animator.SetTrigger("die");
        Scheduler.AddEvent(DestroyOwn, 2);
    }

    private void DestroyOwn()
    {
        Destroy(gameObject);
    }
}
