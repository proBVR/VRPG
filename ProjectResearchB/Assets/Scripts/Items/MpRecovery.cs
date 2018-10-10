using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpRecovery : Item
{
    private readonly int recoveryValue;

    public MpRecovery(string itemName, int id, int recoveryValue) : base(itemName, id)
    {
        this.recoveryValue = recoveryValue;
    }

    public override void Use()
    {
        Player.instance.MpRecover(recoveryValue);
    }
}
