using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : Enemy
{
    //private bool attacking;
    [SerializeField]
    private int[] magics;

    protected override void Move()
    {
        transform.LookAt(Player.instance.transform.position);
        transform.forward -= new Vector3(0, transform.forward.y, 0);
    }

    protected override void Action(int index)
    {
        Scheduler.instance.AddEvent(3, FinAtk);
        GameManager.instance.magicList[index].Use(this);
        actNum = (actNum + 1) % 3;
    }

    protected override void Idle() { }
    protected override void Stop() { }
}
