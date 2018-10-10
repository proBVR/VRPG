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

    protected void Start()
    {
        transform.localPosition = startPosition;
        transform.localEulerAngles = startAngle;
    }

    protected void Update()
    {
        if (counter > 0) counter--;
    }

    public abstract void Skill();

    protected void OnTriggerEnter(Collider other)
    {
        if(counter == 0 && other.gameObject.tag == "Enemy")
        {
            counter = interval;
            other.GetComponent<Enemy>().Damage(attack);
            //Debug.Log("hit: te, left: " + other.GetComponent<Enemy>().GetHp());
        }
    }
}

