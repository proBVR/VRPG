using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    [SerializeField]
    protected bool running;
    [SerializeField]
    private Animator animator;
    //private bool attacking = false;


    protected override void Action(int index)
    {
        
    }

    protected override void Move()
    {
       
        animator.SetBool("running", running);
    }

    protected override void Death()
    {
        throw new System.NotImplementedException();
    }
}
