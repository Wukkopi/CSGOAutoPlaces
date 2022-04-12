using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

namespace CSGOAutoPlaces.Vmf
{
    public struct VisGroup
    {
        public string Name;
        public uint VisGroupId;

        public static VisGroup ParseVisGroup(string visgroupBlock)
        {
            var result = new VisGroup();
            result.VisGroupId = VmfParser.ParseVisGroupId(visgroupBlock);
            result.Name = ParseName(visgroupBlock);
            return result;
        }
        private static string ParseName(string block)
        {
            var lines = block.Split('\n');
            foreach (var line in lines)
            {
                if (line.Contains("name"))
                {
                    var match = Regex.Match(line, "name\" \"(.+)\"");
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }
            }
            return string.Empty;
        }
    }
    public struct Solid
    {
        public List<Vector3> Vertices { get; private set; }
        public uint VisGroupId { get; private set; }
        public AABB AABB { get; private set; }

        private void ParseVertices(string solidBlock)
        {
            Vertices = new List<Vector3>();
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
                        for(var i = 0; i < 9; i+=3)
                        {
                            Vertices.Add(new Vector3(coords[i], coords[i + 1], coords[i + 2]));
                        }
                    }
                }
            }
            Vertices = Vertices.Distinct().ToList();
        }
        private void ParseAABB()
        {
            Vector3 min = new Vector3();
            Vector3 max = new Vector3();
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

    public class VmfParser
    {
        public List<Solid> Solids { get; private set; }
        public List<VisGroup> VisGroups { get; private set; }
        private void ParseSolids(string file)
        {
            var lines = file.Split("\n");
            var curlyCounter = 0;
            var inSolid = false;
            var solidBlock = string.Empty;
            foreach(var s in lines)
            {
                if (!inSolid && s.Trim() == "solid")
                {
                    inSolid = true;
                    curlyCounter = 0;
                    solidBlock = string.Empty;
                    continue;
                }
                if (inSolid)
                {
                    solidBlock += $"{s}\n";
                    if (s.Trim() == "{")
                    {
                        curlyCounter++;
                    }
                    else if (s.Trim() == "}")
                    {
                        if (--curlyCounter == 0)
                        {
                            // end solid and parse it
                            var solid = Solid.ParseSolid(solidBlock);
                            if (solid.VisGroupId != 0)
                            {
                                Solids.Add(solid);
                            }
                            inSolid = false;
                        }
                    }
                }
            }
        }

        private void ParseVisGroups(string file)
        {
            var filePos = 0;
            var visGroupPos = 0;
            do
            {
                // +13 = \n\tvisgroup\n\t{
                visGroupPos = file.IndexOf("\n\tvisgroup", filePos);
                if (visGroupPos != -1)
                {
                    // visgroup found
                    var solidEnd = file.IndexOf("\n\t}", visGroupPos);
                    var block = file.Substring(visGroupPos + 13, solidEnd - visGroupPos);
                    var g = VisGroup.ParseVisGroup(block);
                    if (g.Name.StartsWith("ap_"))
                    {
                        VisGroups.Add(g);
                    }

                    filePos = solidEnd;
                }
            } while (visGroupPos != -1);
        }


        public VmfParser(string filePath)
        {
            var vmfFile = File.ReadAllText(filePath);
            Solids = new List<Solid>();
            VisGroups = new List<VisGroup>();

            // Parse solids first
            ParseSolids(vmfFile);
            ParseVisGroups(vmfFile);
        }

        public static uint ParseVisGroupId(string solidBlock)
        {
            var lines = solidBlock.Split('\n');
            foreach (var line in lines)
            {
                if (line.Contains("visgroupid"))
                {
                    var match = Regex.Match(line, "([-0-9]+)");
                    if (match.Success)
                    {
                        var captures = match.Captures; // (x,y,z) (x2,y2,z2) (x3,y3,z3)
                        return uint.Parse(captures[0].Value);
                    }
                }
            }
            return 0;
        }
    }
}
