using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    [SerializeField]
    protected float search;
    protected override void Action(int index)
    {
        throw new System.NotImplementedException();
    }

    protected override void Move()
    {
        var distance = Vector3.Distance(Player.instance.transform.position, transform.position);
        if(distance < search)
        {

        }
    }

    protected override void Death()
    {
        throw new System.NotImplementedException();
    }
}
