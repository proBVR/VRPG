using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    private float atkSpeed = 8;

    [SerializeField]
    private AtkObject tackle;

    protected override void Action(int index)
    {
        transform.LookAt(Player.instance.transform.position);
        transform.forward -= new Vector3(0, transform.forward.y, 0);
        rb.velocity = transform.forward * atkSpeed;
        animator.SetBool("running", true);

        tackle.AtKBegin(status.Str, AttackAttribute.normal, 0, 4);
        Scheduler.instance.AddEvent(4, FinAtk);        
    }

    protected override void Move()
    {
        transform.LookAt(Player.instance.transform.position);
        transform.forward -= new Vector3(0, transform.forward.y, 0);
        rb.velocity = transform.forward * mvSpeed;
        animator.SetBool("running", true);
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
}
