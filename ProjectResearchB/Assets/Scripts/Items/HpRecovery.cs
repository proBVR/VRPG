using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpRecovery : Item
{
    private readonly int recoveryValue;

    public HpRecovery(string itemName, int id, int recoveryValue) : base(itemName, id)
    {        
        this.recoveryValue = recoveryValue;        
    }

    public override void Use()
    {
        Player.instance.HpRecover(recoveryValue);
    }
}
