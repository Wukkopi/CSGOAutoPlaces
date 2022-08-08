using MIConvexHull;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces.Misc
{
    public static class Extensions
    {
        public static Vector3 ToVector3(this double[] c)
        {
            return new Vector3((float)c[0], (float)c[1], (float)c[2]);
        }

        public static Vector3 ToVector3(this DefaultVertex c)
        {
            return c.Position.ToVector3();
        }
    }
}
