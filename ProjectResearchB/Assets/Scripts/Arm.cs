using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Arm : MonoBehaviour
{
    [SerializeField]
    protected int attack;

    protected abstract void Skill();
}

