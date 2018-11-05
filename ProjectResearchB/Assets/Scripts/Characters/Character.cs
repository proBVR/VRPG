using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{ 
    protected CharacterStatus status;

    public void Init(int hp, int mp, int str, int vit, int speed, AttackAttribute weak)
    {
        status = new CharacterStatus(Death, hp, mp, str, vit, speed, weak);
    }

    public CharacterStatus GetStatus()
    {
        return status;
    }

    protected abstract void Move();
    protected abstract void Action(int index);
    protected abstract void Death();   
}
