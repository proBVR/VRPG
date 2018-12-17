using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus
{
    public readonly string name;
    private int hp, maxHp, mp, maxMp, str, vit, speed;
    private readonly Action death;
    public AttackAttribute weak;

    public int MaxHp
    {
        get { return maxHp; }
        set
        {
            maxHp += value;
            if (maxHp < 0) maxHp = 0;
            if (maxHp < hp) hp = maxHp;
        }
    }

    public int Hp
    {
        get; private set;
    }

    public int MaxMp
    {
        get { return maxMp; }
        set
        {
            maxMp += value;
            if (maxMp < 0) maxMp = 0;
            if (maxMp < mp) hp = maxMp;
        }
    }

    public int Mp
    {
        get { return mp; }
        set
        {
            mp += value;
            if (mp < 0) mp = 0;
            else if (mp > maxMp) mp = maxMp;
        }
    }

    public int Str
    {
        get { return str; }
        set
        {
            str += value;
            if (str < 0) str = 0;
        }
    }

    public int Vit
    {
        get { return vit; }
        set
        {
            vit += value;
            if (vit < 0) vit = 0;
        }
    }

    public int Speed
    {
        get { return speed; }
        set
        {
            speed += value;
            if (speed < 0) speed = 0;
        }
    }

    public CharacterStatus(string name, int hp, int mp, int str, int vit, AttackAttribute weak)
    {
        this.name = name;
        maxHp = hp;
        maxMp = mp;
        this.hp = hp;
        this.mp = mp;
        this.str = str;
        this.vit = vit;
        this.weak = weak;
    }

    public void RecoverHp(int value)
    {
        if (value < 0)
        {
            Debug.Log("error: recover-> " + value);
            return;
        }
        hp += value;
        if (hp > maxHp) hp = maxHp;
    }

    public void Damage(int value, AttackAttribute attribute)
    {
        if (value < 0)
        {
            Debug.Log("error: damage-> " + value);
            return;
        }
        //damage caluclate
        if (attribute == weak) value *= 2;
        if (vit < value)
        {
            hp -= value - vit;
            if (hp <= 0)
            {
                hp = 0;
                death();
            }
        }
    }

    public override string ToString()
    {
        return "HP: "+hp+"/"+maxHp+"\nMP: "+"mp"+"/"+maxMp+"\nSTR: "+str+"\nVIT: "+vit+"\nSPEED: "+speed;
    }
}
