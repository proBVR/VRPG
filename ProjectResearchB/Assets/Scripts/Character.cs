using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {
    protected int hp, mp;
    [SerializeField]
    protected int maxHp, maxMp, str, vit, speed;

    protected void Initialize()
    {
        hp = maxHp;
        mp = maxMp;
    }

    public void Damage(int damage)
    {
        if (damage < vit) return;
        hp -= damage - vit;
        if (hp < 0)
        {
            hp = 0;
            Death();
        }
    }

    public void UseMp(int dec)
    {
        mp -= dec;
        if (mp < 0) mp = 0;
    }

    protected abstract void Move();
    protected abstract void Action();
    protected abstract void Death();   
}
