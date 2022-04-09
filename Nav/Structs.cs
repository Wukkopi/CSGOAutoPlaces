using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct ApproachInfo
    {
        public uint TargetAreaId;
        public uint ApproachPrev;
        public byte Type;
        public uint Next;
        public byte Method;
    }
}
