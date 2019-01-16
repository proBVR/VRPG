using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy, IDamage
{
    //private bool attacking = false;
    //[SerializeField]
    private float time = 5;
    private float attackCounter, atkSpeed = 8;

    protected override void Action(int index)
    {
        attacking = true;
        transform.LookAt(Player.instance.transform.position);
        transform.forward -= new Vector3(0, transform.forward.y, 0);
        rb.velocity = transform.forward * atkSpeed;
        //attackCounter = time;
        Scheduler.instance.AddEvent(time, FinAtk2);
        animator.SetBool("running", true);
    }

    public void FinAtk2()
    {
        if (!attacking) return;
        FinAtk();
        animator.SetBool("running", false);
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

    public int GetPower()
    {
        return status.Str;
    }

    public AttackAttribute GetAttribute()
    {
        return AttackAttribute.normal;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("tag: "+collision.gameObject.tag);
        if (attacking && collision.gameObject.tag == "Player")
        {
            Debug.Log("attack");
            collision.gameObject.GetComponent<Player>().GetStatus().Damage(this);
            FinAtk2();
            attacking = false;
            Idle();
        }
    }
}
