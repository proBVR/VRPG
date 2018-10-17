using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public static Node root;
    private Node[] children = new Node[6];
    private int magic = -1;

    public Node(int num)
    {
        magic = num;
    }

    public Node(Node[] nodes)
    {
        children = nodes;
    }

    public Node NextNode(int index)
    {
        return children[index];
    }

    public int GetMagic()
    {
        return magic;
    }
}
