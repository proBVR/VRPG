using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//はやぶさギリ用のスクリプト
public class SkillStay : SkillObject
{
    private List<Enemy> hits;

	// Use this for initialization
	void Start ()
    {
        transform.parent = Player.instance.GetArm(true).transform;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        hits = new List<Enemy>();
	}

    public override void Activate()
    {
        foreach (Enemy enemy in hits)
        {
            enemy.GetStatus().Damage(power, AttackAttribute.normal);
            enemy.GetStatus().Damage(power, AttackAttribute.normal);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") hits.Add(other.GetComponent<Enemy>());
    }

    public override void Reset()
    {
        hits.Clear();
    }
}
