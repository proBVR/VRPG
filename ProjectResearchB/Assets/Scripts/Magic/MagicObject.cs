using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicObject : MonoBehaviour {
    private int attribute, power, range=1;

    public void Init(int attribute, int power)
    {
        this.attribute = attribute;
        this.power = power;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().Damage(power);
        }
    }

    public abstract Vector3 GetPos();
    public abstract Quaternion GetRot();
}
