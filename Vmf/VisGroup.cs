using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
}
