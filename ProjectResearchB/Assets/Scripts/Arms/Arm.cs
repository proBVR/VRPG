using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Arm : MonoBehaviour
{
    [SerializeField]
    protected int attack, interval;
    [SerializeField]
    protected Vector3 startPosition, startAngle; 
    protected int counter = 0;
    protected Action preMove = null;
    //protected Action activate = null;

    protected void Start()
    {
        //transform.localPosition = startPosition;
        //transform.localEulerAngles = startAngle;
    }

    protected void Update()
    {
        var pos = transform.position - Player.instance.transform.position;
        Debug.Log("pos: " + pos.x.ToString("F1") + ", " + pos.y.ToString("F1"));
        if (counter > 0) counter--;
        if(preMove != null) preMove();
    }

    public void BeginSkill(Action preMove)
    {
        this.preMove = preMove;        
    }

    public void FinSkill()
    {
        preMove = null;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(counter == 0 && other.gameObject.tag == "Enemy")
        {
            counter = interval;
            other.GetComponent<Enemy>().GetStatus().Damage(attack, AttackAttribute.none);
            //Debug.Log("hit: te, left: " + other.GetComponent<Enemy>().GetHp());
        }
    }
}

