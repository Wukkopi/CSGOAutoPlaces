using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces.Misc
{
    public struct Triangle
    {
        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }
        public Vector3 C { get; private set; }
        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
