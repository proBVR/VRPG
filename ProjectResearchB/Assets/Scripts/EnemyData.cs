using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    [SerializeField]
    private string name;
    [SerializeField]
    private int hp, mp, str, vit;
    [SerializeField]
    private AttackAttribute weak;

    public CharacterStatus GenStatus()
    {
        return new CharacterStatus(name, hp, mp, str, vit, weak);
    }
}
