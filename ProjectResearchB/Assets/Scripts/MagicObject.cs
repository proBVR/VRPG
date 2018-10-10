using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicObject : MonoBehaviour {
    private int damage;

    public void Init(int value)
    {
        damage = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().Damage(damage);
        }
    }
}
