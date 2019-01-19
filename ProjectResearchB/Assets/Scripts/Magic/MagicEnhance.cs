using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicEnhance : MagicObject
{
    [SerializeField]
    private float time;

    public override void Activate()
    {
        //属性によって上昇させるパラメータを変えるかも

        user.GetStatus().StrRate += 0.5f;
        Scheduler.AddEvent(FinEnhance, time);
        Destroy(this, time);
    }

    private void FinEnhance()
    {
        user.GetStatus().StrRate -= 0.5f;
    }
}