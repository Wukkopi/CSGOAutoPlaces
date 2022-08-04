using CSGOAutoPlaces.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace CSGOAutoPlaces.Vmf
{
    public struct Solid
    {
        public List<Vector3> Vertices { get; private set; }
        public List<Triangle> Triangles { get; private set; }
        public uint VisGroupId { get; private set; }
        public AABB AABB { get; private set; }

        private void ParseVertices(string solidBlock)
        {
            Vertices = new List<Vector3>();
            Triangles = new List<Triangle>();
            var lines = solidBlock.Split('\n');
            foreach (var line in lines)
            {
                if (line.Contains("plane"))
                {
                    var match = Regex.Matches(line, "([-0-9]+)");// (x,y,z) (x2,y2,z2) (x3,y3,z3)
                    if (match.Count > 0)
                    {
                        var coords = new float[9];
                        for (var i = 0; i < 9; i++)
                        {
                            coords[i] = float.Parse(match[i].Value);
                        }
                        for (var i = 0; i < 9; i += 3)
                        {
                            Vertices.Add(new Vector3(coords[i], coords[i + 1], coords[i + 2]));
                        }
                        Triangles.Add(new Triangle(Vertices[^3], Vertices[^2], Vertices[^1]));
                    }
                }
            }
            Vertices = Vertices.Distinct().ToList();
        }
        private void ParseAABB()
        {
            Vector3 min = Vector3.One * float.MaxValue;
            Vector3 max = Vector3.One * float.MinValue;
            foreach (var v in Vertices)
            {
                min.X = Math.Min(v.X, min.X);
                min.Y = Math.Min(v.Y, min.Y);
                min.Z = Math.Min(v.Z, min.Z);
                max.X = Math.Max(v.X, max.X);
                max.Y = Math.Max(v.Y, max.Y);
                max.Z = Math.Max(v.Z, max.Z);
            }
            AABB = new AABB()
            {
                Min = min,
                Max = max
            };
        }

        public static Solid ParseSolid(string solidBlock)
        {
            var result = new Solid();
            result.VisGroupId = VmfParser.ParseVisGroupId(solidBlock);
            if (result.VisGroupId == 0)
            {
                return result;
            }
            result.ParseVertices(solidBlock);
            result.ParseAABB();
            return result;
        }
    }
}
