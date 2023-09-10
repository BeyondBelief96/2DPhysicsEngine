using System.Diagnostics.CodeAnalysis;

namespace FlatPhysics
{
    public readonly struct FlatVector
    {
        public readonly float X;
        public readonly float Y;

        public FlatVector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static readonly FlatVector Zero = new FlatVector(0f, 0f);

        public static FlatVector operator +(FlatVector a, FlatVector b)
        {
            return new FlatVector(a.X + b.X, a.Y + b.Y);
        }

        public static FlatVector operator -(FlatVector a, FlatVector b)
        {
            return new FlatVector(a.X - b.X, a.Y - b.Y);
        }

        public static FlatVector operator *(FlatVector v, float s)
        {
            return new FlatVector(s*v.X, s*v.Y);
        }

        public static FlatVector operator /(FlatVector v, float s)
        {
            return new FlatVector(v.X / s, v.Y / s);
        }

        public static FlatVector operator -(FlatVector v)
        {
            return new FlatVector(-v.X, -v.Y);
        }

        public bool Equal(FlatVector other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if(obj is FlatVector other)
            {
                return this.Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return new { this.X, this.Y }.GetHashCode();
        }

        public override string ToString()
        {
            return $"X: {this.X}, Y: {this.Y}";
        }
    }
}