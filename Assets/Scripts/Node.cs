using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IComparable
{
    public int x { get; set; }
    public int y { get; set; }

    public float fCost { get; set; }
    public float gCost { get; set; }
    public float hCost { get; set; }

    public bool walkable;
    public GameObject img;

    public Node from;

    public Node (int _x, int _y) {
        x = _x;
        y = _y;
        fCost = 0f;
        gCost = 0f;
        hCost = 0f;
        walkable = true;
        from = null;
    }

    public void Initialize()
    {
        fCost = 0f;
        gCost = 0f;
        hCost = 0f;
        from = null;
    }

    public void CalcCost()
    {
        fCost = gCost + hCost;
    }

    public int CompareTo(object obj)
    {
        Node n = (Node)obj;
        n.CalcCost();
        this.CalcCost();

        return this.fCost < n.fCost ? 1 : -1;
    }

    public static bool Equal(Node node1, Node node2)
    {
        if(node1.x == node2.x && node1.y == node2.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
