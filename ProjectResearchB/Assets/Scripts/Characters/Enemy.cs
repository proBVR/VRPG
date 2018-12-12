using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected float counter, mvInterval=1, mvCounter;
    [SerializeField]
    protected float interval;
    [SerializeField]
    protected float searchRange, attackRange;
    protected bool atkFin = false, cooling = false;
    protected EnemyManager manager;
    protected int state, actNum = 0;
    protected Animator animator;
    protected Rigidbody rb;

    /*
     state
     0:idle
     1:move
     2:attack         
    */

    // Use this for initialization
    protected void Start()
    {
        counter = interval;
        mvCounter = mvInterval;
        state = 0;
        manager = transform.parent.GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.ExistPlayer()) return;

        if (cooling)
        {
            counter -= Time.deltaTime;
            if (counter < 0)
            {
                counter = interval;
                cooling = false;
            }
        }

        
        switch (state)
        {
            case 0|1:
                mvCounter -= Time.deltaTime;
                if (mvCounter > 0) break;
                mvCounter = mvInterval;
                var distance = Vector3.Distance(Player.instance.transform.position, transform.position);
                if (!cooling && distance < attackRange) state = 2;
                else if (state == 1)
                {
                    Move();
                    if (distance > searchRange || attackRange > distance) state = 0;
                }
                else
                {
                    Idle();
                    if (attackRange < distance && distance < searchRange) state = 1;
                }
                break;
            case 2:
                Action(actNum);
                if (atkFin) state = 0;
                break;
            default:
                break;
        } 
    }

    protected abstract void Idle();
}