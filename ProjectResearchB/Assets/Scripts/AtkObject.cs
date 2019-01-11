using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkObject : MonoBehaviour
{
    private int power;
    private AttackAttribute attribute;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void AtKBegin(int power, AttackAttribute attribute, float start, float end)
    {
        this.power = power;
        this.attribute = attribute;
        Scheduler.instance.AddEvent(start, Begin);
        Scheduler.instance.AddEvent(end, Finish);
    }

    private void Begin()
    {
        gameObject.SetActive(true);
    }

    private void Finish()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Character>().GetStatus().Damage(power, attribute);
        }
    }
}
