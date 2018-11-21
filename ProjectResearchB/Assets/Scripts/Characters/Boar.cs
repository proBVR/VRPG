using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    [SerializeField]
    protected bool running;
    [SerializeField]
    private Animator animator;
    private bool attacking = false;


    protected override void Action(int index)
    {
        //var distance = Vector3.Distance(Player.instance.transform.position, transform.position);
        //if(distance < search)
    }

    protected override void Move()
    {
        /*
        var distance = Vector3.Distance(Player.instance.transform.position, transform.position);
        if(distance < search)
        {

        }
        */
        animator.SetBool("running", running);
    }

    protected override void Death()
    {
        throw new System.NotImplementedException();
    }
}
