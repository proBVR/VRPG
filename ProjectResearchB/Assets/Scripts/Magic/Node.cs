using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public static Node root;
    private Node[] children = new Node[6];
    private Magic magic;

    public Node(Magic magic)
    {
        this.magic = magic;
    }

    public Node(Node[] nodes)
    {
        children = nodes;
    }

    public Node NextNode(int index)
    {
        return null;
    }
}
