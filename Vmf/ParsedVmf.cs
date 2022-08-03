using System;
using System.Collections.Generic;
using System.IO;

namespace CSGOAutoPlaces.Vmf
{
    class ParsedVmf
    {
        public List<Solid> Solids { get; private set; }
        public List<VisGroup> VisGroups { get; private set; }

        public ParsedVmf(string filePath)
        {
            var vmfFile = File.ReadAllText(filePath);

            Solids = VmfParser.ParseSolids(vmfFile);
            VisGroups = VmfParser.ParseVisGroups(vmfFile);
        }

    }
}
