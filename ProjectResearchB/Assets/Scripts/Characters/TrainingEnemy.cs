using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEnemy : Enemy
{
    private int count = 0;

    protected override void Move() { }

    protected override void Action(int index)
    {

    }

    protected override void Death()
    {
        count++;
        Debug.Log("training enemy died: "+count);
        status["hp"] = status["maxHp"];
    }
}
