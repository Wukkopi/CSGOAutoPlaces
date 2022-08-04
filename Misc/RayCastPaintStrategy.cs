using CSGOAutoPlaces.Interfaces;
using CSGOAutoPlaces.Nav;
using CSGOAutoPlaces.Vmf;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces.Misc
{
    class RayCastPaintStrategy : IPaintStrategy
    {
        public void PaintPlaceNames(ParsedVmf vmf, NavFile nav, List<Solid> apSolids)
        {
            for (var i = 0; i < nav.Areas.Length; i++)
            {
                Vector3 navCenter = (nav.Areas[i].NorthWestCorner + nav.Areas[i].SouthEastCorner) / 2f;
                float? bestHit = null;
                Solid? bestSolid = null;
                foreach (var solid in apSolids)
                {
                    foreach (var triangle in solid.Triangles)
                    {
                        // raycast down from the center of navarea
                        float? hit = IntersectRayTriangle(new Ray(navCenter, -Vector3.UnitY), triangle);
                        if (hit != null && hit < bestHit)
                        {
                            bestHit = hit;
                            bestSolid = solid;
                        }
                    }
                }
                if (bestSolid != null)
                {
                    // PIHVI
                    // +1 because placename 0 is "no name"
                    var id = vmf.VisGroups.FindIndex(v => v.VisGroupId == bestSolid?.VisGroupId) + 1;
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

        // snatched and modified from https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Utils/MathUtils.cs#L274
        private static float? IntersectRayTriangle(Ray ray, Triangle t)
        {
            Vector3 ab = t.B - t.A;
            Vector3 ac = t.C - t.A;

            // Compute triangle normal. Can be precalculated or cached if
            // intersecting multiple segments against the same triangle
            Vector3 n = Vector3.Cross(ab, ac);

            // Compute denominator d. If d <= 0, segment is parallel to or points
            // away from triangle, so exit early
            float d = Vector3.Dot(-ray.Direction, n);
            if (d <= 0.0f) return null;

            // Compute intersection t value of pq with plane of triangle. A ray
            // intersects iff 0 <= t. Segment intersects iff 0 <= t <= 1. Delay
            // dividing by d until intersection has been found to pierce triangle
            Vector3 ap = ray.Origin - t.A;
            float h = Vector3.Dot(ap, n);
            if (h < 0.0f) return null;
            //if (t > d) return null; // For segment; exclude this code line for a ray test

            // Compute barycentric coordinate components and test if within bounds
            Vector3 e = Vector3.Cross(-ray.Direction, ap);
            float v = Vector3.Dot(ac, e);
            if (v < 0.0f || v > d) return null;

            float w = -Vector3.Dot(ab, e);
            if (w < 0.0f || v + w > d) return null;

            // Segment/ray intersects triangle. Perform delayed division and
            // compute the last barycentric coordinate component
            float ood = 1.0f / d;
            h *= ood;

            return h;
        }
    }
}
