using System;
using UnityEngine;

public class GridPoint : IEquatable<GridPoint>
{
    public Point Point;
    public Color Color;

    public GridPoint(Point point, Color color)
    {
        Point = point;
        Color = color;
        Color.a = 0.7f;
    }

    public bool Equals(GridPoint other)
    {
        return Equals(other.Point, Point);
    }
}