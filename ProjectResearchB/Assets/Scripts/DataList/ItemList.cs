using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data List/Item")]
public class ItemList : ScriptableObject
{
    public Item[] list;
}
