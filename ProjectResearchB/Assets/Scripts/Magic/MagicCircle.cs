using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MagicObject
{
    [SerializeField]
    private float range;

    public override void Activate()
    {
        GetComponent<Renderer>().material = GameManager.instance.materials[(int)attribute];

        range *= (1 + 0.25f * (rangeRate - 1));
        transform.localScale *= range;
        transform.position = user.transform.position;
        Destroy(this, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ((user.IsPlayer) ? "Enemy" : "Player"))
        {
            other.GetComponent<Character>().GetStatus().Damage(this);
        }
    }
}
