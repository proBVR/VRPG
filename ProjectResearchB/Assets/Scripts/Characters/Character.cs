﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{ 
    protected CharacterStatus status;

    public void Init(CharacterStatus status)
    {
        this.status = status;
    }

    public CharacterStatus GetStatus()
    {
        return status;
    }

    protected abstract void Move();
    protected abstract void Action(int index);
    protected abstract void Death();   
}
