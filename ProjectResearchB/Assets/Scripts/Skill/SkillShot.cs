using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//まじんケン用のスクリプト
public class SkillShot : SkillObject
{
   // private Vector3 dir;
    private int speed = 1;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }

    public override void Activate()
    {
        var dir = user.transform.forward;
        transform.position = user.transform.position + dir + Vector3.up;
        transform.forward = dir;
        rb.velocity = speed * dir;
    }

    public override void Reset() { }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == ((user.IsPlayer) ? "Enemy" : "Player"))
        {
            other.GetComponent<Character>().GetStatus().Damage(this);
        }
    }
}