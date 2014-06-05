using System;
using UnityEngine;

public class GridPoint : IEquatable<GridPoint>
{
    public Vector3 Position;
    public Color Color;

    public GridPoint(Vector3 position, Color color)
    {
        Position = position;
        Color = color;
        Color.a = 0.7f;
    }

    public bool Equals(GridPoint other)
    {
        return other.Position == Position;
    }
}