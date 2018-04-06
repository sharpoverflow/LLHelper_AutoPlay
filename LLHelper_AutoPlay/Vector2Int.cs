using System;
using System.Drawing;

public struct Vector2Int
{
    public static Vector2Int Zero
    {
        get
        {
            return new Vector2Int(0, 0);
        }
    }
    public static Vector2Int One
    {
        get
        {
            return new Vector2Int(1, 1);
        }
    }

    public int x;
    public int y;

    public float Length
    {
        get
        {
            return (float)Math.Sqrt(LengthMagnitude);
        }
    }

    public float LengthMagnitude
    {
        get
        {
            return x * x + y * y;
        }
    }

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.Equals(b);
    }
    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !a.Equals(b);
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x + b.x, a.y + b.y);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x - b.x, a.y - b.y);
    }

    public static Vector2Int operator *(Vector2Int a, int b)
    {
        return new Vector2Int(a.x * b, a.y * b);
    }

    public static implicit operator Point(Vector2Int p)
    {
        return new Point(p.x, p.y);
    }

    public static implicit operator Size(Vector2Int s)
    {
        return new Size(s.x, s.y);
    }

    public override bool Equals(object obj)
    {
        Vector2Int v = (Vector2Int)obj;
        return x == v.x && y == v.y;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static float Distance(Vector2Int a, Vector2Int b)
    {
        return (a - b).Length;
    }

    public static float DistanceMagnitude(Vector2Int a, Vector2Int b)
    {
        return (a - b).LengthMagnitude;
    }
}
