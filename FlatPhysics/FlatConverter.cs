using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace FlatPhysics
{
    public static class FlatConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToVector2(FlatVector v)
        {
            return new Vector2(v.X, v.Y);
        }
    }
}
