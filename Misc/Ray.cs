using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces.Misc
{
    struct Ray
    {
        public Vector3 Origin { get; private set; }
        public Vector3 Direction { get; private set; }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
}
