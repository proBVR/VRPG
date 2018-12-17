using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public static Node root = new Node();
    private Node[] children = new Node[6];
    private int magic = -1;

    public Node NextNode(int index)
    {
        return children[index];
    }

    public int GetMagic()
    {
        return magic;
    }

    public static void AddNode(int[] values, int id)
    {
        var p = root;
        for(int i = 0; i < values.Length; i++)
        {
            if (p.children[values[i]] == null) p.children[values[i]] = new Node();
            p = p.children[values[i]];
        }
        p.magic = id;
    }
}
