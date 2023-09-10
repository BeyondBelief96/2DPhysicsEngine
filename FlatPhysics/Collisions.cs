namespace FlatPhysics
{
    public static class Collisions
    {
        public static bool IntersectCircles(FlatVector centerA, float radiusA, FlatVector centerB,
            float radiusB, out FlatVector normal, out float depth)
        {
            normal = FlatVector.Zero;
            depth = 0;
            float distance = FlatMath.Distance(centerA, centerB);
            float totalRadii = radiusA + radiusB;

            if(distance >= totalRadii)
            {
                return false;
            }

            normal = FlatMath.Normalize(centerB - centerA);
            depth = totalRadii - distance;

            return true;
        }
    }
}
