using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus
{
    public readonly string name;
    private int mp, maxHpBase, maxMpBase, strBase, vitBase;
    private float maxHpRate = 1, maxMpRate = 1, strRate = 1, vitRate = 1;
    private Action death;
    public AttackAttribute weak;

    public List<IDamage> guardList = new List<IDamage>();

    public int Hp { get; private set; }

    public int Mp
    {
        get { return mp; }
        set
        {
            if (mp < 0) mp = 0;
            else if (mp > MaxMp) mp = MaxMp;
            else mp = value;
        }
    }

    public int MaxHp
    { get { return (int)(maxHpRate * maxHpBase); } }

    public int MaxMp
    { get { return (int)(maxMpRate * maxMpBase); } }

    public int Str
    { get { return (int)(strRate * strBase); } }

    public int Vit
    { get { return (int)(vitRate * vitBase); } }

    public float MaxHpRate
    {
        get { return maxHpRate; }
        set
        {
            if (value < 0) maxHpRate = 0;
            else maxHpRate = value;
        }
    }

    public float MaxMpRate
    {
        get { return maxMpRate; }
        set
        {
            if (value < 0) maxMpRate = 0;
            else maxMpRate = value;
        }
    }

    public float StrRate
    {
        get { return strRate; }
        set
        {
            if (value < 0) strRate = 0;
            else strRate = value;
        }
    }

    public float VitRate
    {
        get { return vitRate; }
        set
        {
            if (value < 0) vitRate = 0;
            else vitRate = value;
        }
    }

    public CharacterStatus(string name, int hp, int mp, int str, int vit, AttackAttribute weak)
    {
        this.name = name;
        maxHpBase = hp;
        maxMpBase = mp;
        this.Hp = hp;
        this.Mp = mp;
        this.strBase = str;
        this.vitBase = vit;
        this.weak = weak;
    }

    public void RecoverHp(int value)
    {
        if (value < 0)
        {
            Debug.Log("error: recover-> " + value);
            return;
        }
        Hp += value;
        if (Hp > maxHpBase) Hp = maxHpBase;
    }

    public void Damage(IDamage damager)
    {
        //ガードチェック
        foreach (IDamage temp in guardList)
            if (temp == damager)
            { Debug.Log("guard: success"); return; }

        var value = damager.GetPower();
        if (value < 0)
        {
            Debug.Log("error: damage-> " + value);
            return;
        }
        //damage caluclate
        if (damager.GetAttribute() == weak) value *= 2;
        if (vitBase < value)
        {
            Hp -= value - vitBase;
            if (Hp <= 0)
            {
                Hp = 0;
                death();
            }
        }
    }

    public bool UseMp(int value)
    {
        if (value > mp) return false;
        mp -= value;
        return true;
    }

    public override string ToString()
    {
        return "HP: " + Hp + "/" + MaxHp + "\nMP: " + Mp + "/" + MaxMp + "\nSTR: " + Str + "\nVIT: " + Vit + "\nWEAK: " + weak.ToString();
    }

    public void LevelUp(float rate)
    {
        maxHpBase = (int)(maxHpBase * rate);
        maxMpBase = (int)(maxMpBase * rate);
        strBase = (int)(strBase * rate);
        vitBase = (int)(vitBase * rate);
    }

    public void SetDead(Action death)
    {
        this.death = death;
    }
}
