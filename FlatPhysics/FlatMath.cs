namespace FlatPhysics
{
    public static class FlatMath
    {
        public static float Length(FlatVector v)
        {
            return (float) Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static float Distance(FlatVector v1, FlatVector v2)
        {
            return Length(v2 - v1);
        }

        public static FlatVector Normalize(FlatVector v)
        {
            float length = Length(v);
            return new FlatVector(v.X / length, v.Y / length);
        }

        public static float Dot(FlatVector v1, FlatVector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static float Cross(FlatVector v1, FlatVector v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (min == max) return min;
            if(min > max) throw new ArgumentOutOfRangeException("Min is greater than the max.");

            if (value < min)
                return min;
            if (value > max)
                return max;

            return value;
        }
    }
}
