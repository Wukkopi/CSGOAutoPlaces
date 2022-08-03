using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace CSGOAutoPlaces.Vmf
{
    public class VmfParser
    {
        public static List<Solid> ParseSolids(string file)
        {
            var result = new List<Solid>();
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
                                result.Add(solid);
                            }
                            inSolid = false;
                        }
                    }
                }
            }
            return result;
        }

        public static List<VisGroup> ParseVisGroups(string file)
        {
            var result = new List<VisGroup>();
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
                        result.Add(g);
                    }

                    filePos = solidEnd;
                }
            } while (visGroupPos != -1);
            return result;
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
