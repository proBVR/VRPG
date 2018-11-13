using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected int counter;
    [SerializeField]
    protected int interval;
    protected EnemyManager manager;

    // Use this for initialization
    protected void Start()
    {
        counter = interval;
    }

    // Update is called once per frame
    void Update()
    {
        counter--;
        if (counter == 0)
        {
            Action(0);
            counter = interval;
        }
        Move();
    }
}