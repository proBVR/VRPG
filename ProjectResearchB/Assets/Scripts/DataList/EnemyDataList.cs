using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data List/EnemyData")]
public class EnemyDataList : ScriptableObject
{
    public EnemyData[] list;
}