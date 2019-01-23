using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected static readonly int maxLevel = 50;//暫定的なレベルキャップ

    protected CharacterStatus status;
    //protected string name;
    protected int level;
    protected float luRate;
    public bool IsPlayer { protected set; get; }//PCか否か

    public void Init(CharacterStatus status, int level)
    {
        this.level = level;
        this.status = status;
        status.SetDead(Death);
        for (int i = 0; i < level - 1; i++)
            status.LevelUp(luRate);
    }

    public CharacterStatus GetStatus()
    {
        return status;
    }

    protected abstract void Move();
    protected abstract void Action(int index);
    protected abstract void Death();   
}
