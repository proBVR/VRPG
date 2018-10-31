using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionObject : MonoBehaviour
{
    public abstract Vector3 GetPos();
    public abstract Quaternion GetRot();

    protected void Extinction()
    {
        Player.instance.acting = false;
        Destroy(this);
    }
}
