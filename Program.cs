using CSGOAutoPlaces.Interfaces;
using CSGOAutoPlaces.Misc;
using CSGOAutoPlaces.Nav;
using CSGOAutoPlaces.Vmf;
using System;
using System.IO;
using System.Linq;

namespace CSGOAutoPlaces
{
    class Program
    {
        static void Main(string[] args)
        {
            var deleteFlag = false;
            var vmfFile = string.Empty;
            var navFile = string.Empty;
            IPaintStrategy paintStrategy = null;
            foreach (var s in args)
            {
                if (s == "-delete")
                {
                    // remove old nav file
                    deleteFlag = true;
                }
                else if (s.StartsWith("-vmf="))
                {
                    // vmfFile
                    vmfFile = s.Substring(5);
                }
                else if (s.StartsWith("-nav="))
                {
                    //navFile
                    navFile = s.Substring(5);
                }
                else if (s.StartsWith("-strategy="))
                {
                    // strategy for painting
                    var strategy = s.Substring(10);
                    switch (strategy)
                    {
                        case "aabb":
                            paintStrategy = new AABBPaintStrategy();
                            break;
                        case "raycast":
                            paintStrategy = new RayCastPaintStrategy();
                            break;
                    }
                }
            }

            // assign Raycast painting as default.
            paintStrategy ??= new RayCastPaintStrategy();

            Console.WriteLine($"nav: {navFile} -> {(File.Exists(navFile) ? " Found" : " Not found")}");
            Console.WriteLine($"vmf: {vmfFile} -> {(File.Exists(vmfFile) ? " Found" : " Not found")}");

            if (deleteFlag)
            {
                Console.WriteLine("Deleting nav file...");
                File.Delete(navFile);
                Console.WriteLine("Done");
                return;
            }

            var files = File.Exists(vmfFile) && File.Exists(navFile);

            if (!files)
            {
                Console.WriteLine("Required file wasn't found. Cancelling.");
                return;
            }

            Console.WriteLine("Parsing nav...");
            NavFile nav = new NavFile(navFile);

            Console.WriteLine($"{nav.AreaCount} Nav areas found");

            Console.WriteLine("Parsing vmf for places...\n");
            ParsedVmf vmf = new ParsedVmf(vmfFile);
            
            // insert places
            nav.Places = new PlaceName[vmf.VisGroups.Count];
            nav.PlaceCount = (ushort)vmf.VisGroups.Count;
            
            Console.WriteLine($"{nav.PlaceCount} places found:");
            for(var i = 0; i < nav.PlaceCount; i++)
            {
                nav.Places[i].Name = vmf.VisGroups[i].Name.Substring(3) + "\0";
                nav.Places[i].Length = (ushort)nav.Places[i].Name.Length;
                Console.WriteLine($"\t{nav.Places[i].Name}");
            }

            // filter all solids that have ap_ visgroup. note: if solid has more visgroups assigned, only first one is considered.
            var apSolids = vmf.Solids.FindAll(s => vmf.VisGroups.Select(v => v.VisGroupId).Contains(s.VisGroupId));

            Console.WriteLine($"{apSolids.Count} solids with places found.");

            Console.WriteLine("Applying places...");

            paintStrategy.PaintPlaceNames(vmf, nav, apSolids);

            Console.WriteLine("\nSaving nav file...");
            nav.SaveToFile(navFile);
            Console.WriteLine("Done!");
        }
    }
}
