using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    [SerializeField]
    protected float search;
    [SerializeField]
    protected bool running;
    [SerializeField]
    protected Animator animator;


    protected override void Action(int index)
    {
        throw new System.NotImplementedException();
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
