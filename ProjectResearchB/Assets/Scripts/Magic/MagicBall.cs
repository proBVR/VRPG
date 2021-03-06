﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MagicObject
{
    [SerializeField]
    private float range, speed;
    private Rigidbody rb;

    public override void Activate()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Renderer>().material = GameManager.instance.materials[(int)attribute];

        range *= (1 + 0.5f * (rangeRate - 4));
        Destroy(gameObject, range / speed);
        Debug.Log("range: "+range);

        transform.position = user.transform.position + user.transform.forward + Vector3.up;
        var temp = user.transform.forward;
        temp.y = 0;
        transform.forward = temp;

        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ((user.IsPlayer) ? "Enemy" : "Player"))
        {
            other.GetComponent<Character>().GetStatus().Damage(this);
        }
    }
}
