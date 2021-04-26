using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public PointComponent GridPoint { get; set; }
    public PointManager PointRef { get; private set; }
    public Vector2 CenterPoint { get; private set; }
    public Node Parent { get; private set; }
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    public Node(PointManager pointRef)
    {
        this.PointRef = pointRef;
        this.GridPoint = pointRef.Point;
        this.CenterPoint = pointRef.CenterPoint;
    }

    public void CalcScore(Node parent, PointComponent goalPosition, int gScore)
    {
        this.Parent = parent;
        this.G = parent.G + gScore;
        this.H = (Math.Abs(GridPoint.X - goalPosition.X) + Math.Abs(GridPoint.Y - goalPosition.Y)) * 10;
        this.F = this.G + this.H;
    }
}
