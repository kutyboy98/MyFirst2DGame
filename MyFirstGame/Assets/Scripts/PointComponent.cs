using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointComponent
{
    public int X { get; set; }
    public int Y { get; set; }

    public PointComponent(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static bool operator ==(PointComponent firstP, PointComponent secondP)
    {
        return firstP.X == secondP.X && firstP.Y == secondP.Y;
    }

    public static bool operator !=(PointComponent firstP, PointComponent secondP)
    {
        return firstP.X != secondP.X || firstP.Y != secondP.Y;
    }

    public static PointComponent operator -(PointComponent firstP, PointComponent secondP)
    {
        return new PointComponent(firstP.X - secondP.X, firstP.Y - secondP.Y);
    }
}
