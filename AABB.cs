using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces
{
    public struct AABB
    {
        public Vector3 Min;
        public Vector3 Max;

        public bool CollidesWith(AABB other)
        {
            return
                Max.X > other.Min.X &&
                Min.X < other.Max.X &&
                Max.Y > other.Min.Y &&
                Min.Y < other.Max.Y &&
                Max.Z > other.Min.Z &&
                Min.Z < other.Max.Z;
        }
    }
}
