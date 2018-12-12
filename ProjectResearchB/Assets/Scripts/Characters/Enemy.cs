using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected float counter;
    [SerializeField]
    protected float interval;
    [SerializeField]
    protected float searchRange, attackRange;
    protected bool atkFin = false, cooling = false;
    protected EnemyManager manager;
    protected int state;

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
        state = 0;
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
                var distance = Vector3.Distance(Player.instance.transform.position, transform.position);
                if (!cooling && distance < attackRange) state = 2;
                else if (state == 1)
                {
                    Move();
                    if (distance > searchRange) state = 1;                    
                }
                else if (distance > searchRange) state = 0;
                break;
            case 2:
                Action(0);
                if (atkFin) state = 0;
                break;
            default:
                break;
        } 
    }
}