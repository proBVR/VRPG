using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{    
    private bool attacking = false;
    //[SerializeField]
    private float time = 5;
    private float attackCounter, atkSpeed = 8, mvSpeed=4;

    protected override void Action(int index)
    {
        if (!attacking)
        {
            attacking = true;
            transform.LookAt(Player.instance.transform.position);
            transform.forward -= new Vector3(0, transform.forward.y, 0);
            rb.velocity = transform.forward * atkSpeed;
            //attackCounter = time;
            Scheduler.instance.AddEvent(time, FinAtk);
            animator.SetBool("running", true);
        }
    }

    public void FinAtk()
    {
        atkFin = true;
        attacking = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("tag: "+collision.gameObject.tag);
        if (attacking && collision.gameObject.tag == "Player")
        {
            Debug.Log("attack");
            collision.gameObject.GetComponent<Player>().GetStatus().Damage(status.Str, AttackAttribute.normal);
            state = 0;
            attacking = false;
            Idle();
        }
    }
}
