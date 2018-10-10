using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected int[] initStatus;
    protected Dictionary<string, int> status = new Dictionary<string, int>();

    protected virtual void Start()
    {
        status.Add("maxHp", initStatus[0]);
        status.Add("hp", initStatus[0]);
        status.Add("maxMp", initStatus[1]);
        status.Add("mp", initStatus[0]);
        status.Add("str", initStatus[2]);
        status.Add("vit", initStatus[3]);
        status.Add("speed", initStatus[4]);
        initStatus = null;
    }

    public void Damage(int damage)
    {
        if (damage < status["vit"]) return;
        status["hp"] -= damage - status["vit"];
        if (status["hp"] <= 0)
        {
            status["hp"] = 0;
            Death();
        }
    }

    public void UseMp(int dec)
    {
        status["mp"] -= dec;
        if (status["mp"] < 0) status["mp"] = 0;
    }

    public void HpRecover(int value)
    {
        status["hp"] += value;
        if (status["hp"] > status["maxHp"]) status["hp"] = status["maxHp"];
    }

    public void MpRecover(int value)
    {
        status["mp"] += status["maxMp"];
        if (status["mp"] > status["maxMp"]) status["mp"] = status["maxMp"];
    }

    public int GetStatus(string key)
    {
        if (status.ContainsKey(key)) return status[key];
        Debug.Log("the key does not exist: " + key);
        return 0;
    }

    protected abstract void Move();
    protected abstract void Action(int index);
    protected abstract void Death();   
}
