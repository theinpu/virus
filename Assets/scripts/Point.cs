
using System;
public struct Point : IEquatable<Point>
{
	public int X;
	public int Y;

	public Point (int x, int y)
	{
		this.X = x;
		this.Y = y;
	}

    public bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }

    public override string ToString()
    {
        return X + "x" + Y;
    }
}