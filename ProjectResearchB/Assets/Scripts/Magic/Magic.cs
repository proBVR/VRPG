using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Magic : IActionable
{
    private enum Rank { low, middle, high }
    private enum Model { ball, around, area, enhance }
    private enum Range { ShortRange = 3, MiddleRange = 4, LongRange = 5 }

    [SerializeField]
    private string name;
    [SerializeField]
    private Rank rank;
    [SerializeField]
    private Model model;
    [SerializeField]
    private Range range;
    [SerializeField]
    private int power, cost;
    private int time = 10;
    [SerializeField]
    protected AttackAttribute attribute;

    public void Use(Character user)
    {
        Debug.Log("use magic: " + name);
        var prefab = GameManager.instance.GenMagic((int)model);
        prefab.Init(attribute, power, (int)range, user);
    }

    public string GetName()
    {
        return name;
    }

    public void RegisterNode(int id)
    {
        List<int> temp = new List<int>();
        temp.Add((int)rank);
        temp.Add((int)attribute);
        switch (rank)
        {
            case Rank.high:
                temp.Add((int)model);
                temp.Add((int)range);
                break;
            case Rank.middle:
                temp.Add((int)model);
                break;
        }
        Node.AddNode(temp.ToArray(), id);
    }

    public int GetCost()
    {
        return cost;
    }
}