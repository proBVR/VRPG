using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected int counter;
    [SerializeField]
    protected int interval;
    [SerializeField]
    protected float searchRange, attackRange;
    protected bool attacking;
    protected EnemyManager manager;

    // Use this for initialization
    protected void Start()
    {
        counter = interval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.ExistPlayer()) return;
        var distance = Vector3.Distance(Player.instance.transform.position, transform.position);
        if (distance < attackRange && !attacking)
        {
            counter--;
            if (counter == 0)
            {
                Action(0);
                counter = interval;
            }
        }
        else if (distance < searchRange) Move();        
    }
}