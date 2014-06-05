using UnityEngine;

public struct GridPoint
{
    public Vector3 Position;
    public Color Color;

    public GridPoint(Vector3 position, Color color)
    {
        Position = position;
        Color = color;
        Color.a = 0.7f;
    }
}