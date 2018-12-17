using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{    
    private bool attacking = false;
    [SerializeField]
    private float time = 2;
    private float attackCounter, atkSpeed = 2, mvSpeed=0.5f;

    protected override void Action(int index)
    {
        if (!attacking)
        {
            attacking = true;
            transform.LookAt(Player.instance.transform.position);
            rb.velocity = transform.forward * atkSpeed;
            attackCounter = time;
            animator.SetBool("running", true);
        }
        else
        {
            attackCounter -= Time.deltaTime;
            if (attackCounter < 0)
            {
                atkFin = true;
                animator.SetBool("running", false);
            }
        }
    }

    protected override void Move()
    {
        transform.LookAt(Player.instance.transform.position);
        rb.velocity = transform.forward * mvSpeed;
        animator.SetBool("running", true);
    }

    protected override void Idle()
    {
        animator.SetBool("runing", false);
        rb.velocity = Vector3.zero;
    }

    protected override void Death()
    {
        Debug.Log("death: Rhino");
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (attacking && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Character>().GetStatus().Damage(status.Str, AttackAttribute.normal);
        }
    }
}
