using CSGOAutoPlaces.Interfaces;
using CSGOAutoPlaces.Nav;
using CSGOAutoPlaces.Vmf;
using System;
using System.Collections.Generic;

namespace CSGOAutoPlaces.Misc
{
    class AABBPaintStrategy : IPaintStrategy
    {
        public void PaintPlaceNames(ParsedVmf vmf, NavFile nav, List<Solid> apSolids)
        {
            for (var i = 0; i < nav.Areas.Length; i++)
            {
                foreach (var solid in apSolids)
                {
                    if (nav.Areas[i].GetAABB().CollidesWith(solid.AABB))
                    {
                        // PIHVI
                        // +1 because placename 0 is "no name"
                        var id = vmf.VisGroups.FindIndex(v => v.VisGroupId == solid.VisGroupId) + 1;
                        nav.Areas[i].PlaceId = (ushort)id;
                        Console.Write("*");
                        break;
                    }
                    else
                    {
                        // unset place
                        nav.Areas[i].PlaceId = 0;
                        Console.Write(".");
                    }
                }
            }
        }
    }
}
