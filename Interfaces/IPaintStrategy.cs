using CSGOAutoPlaces.Nav;
using CSGOAutoPlaces.Vmf;
using System;
using System.Collections.Generic;

namespace CSGOAutoPlaces.Interfaces
{
    interface IPaintStrategy
    {
        public void PaintPlaceNames(ParsedVmf vmf, NavFile nav, List<Solid> apSolids);
    }
}
