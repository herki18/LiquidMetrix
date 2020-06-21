using System;

namespace LiquidMetrix.Data
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public static Vector2 Zero;

        public int X { get; }
        public int Y { get; }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj != null && Equals((Vector2) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs) { 
            if(lhs.X == rhs.X && lhs.Y == rhs.Y)
                return true;
            return false;
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }


        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}
