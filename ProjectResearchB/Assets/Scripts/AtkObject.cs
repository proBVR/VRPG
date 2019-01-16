using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵用のスキル的な位置づけ
public class AtkObject : MonoBehaviour, IDamage
{
    private int power;
    private AttackAttribute attribute;

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

    public int GetPower()
    {
        return power;
    }

    public AttackAttribute GetAttribute()
    {
        return attribute;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Character>().GetStatus().Damage(this);
        }
    }
}
