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
            foreach (var s in args)
            {
                if (s == "-d")
                {
                    // remove old nav file
                    deleteFlag = true;
                }
                if (s.StartsWith("-vmf="))
                {
                    // vmfFile
                    vmfFile = s.Substring(5);
                }
                if (s.StartsWith("-nav="))
                {
                    //navFile
                    navFile = s.Substring(5);
                }
            }

            Console.WriteLine($"nav: {navFile} -> {(File.Exists(navFile) ? " Found" : " Not found")}");

            if (deleteFlag)
            {
                Console.WriteLine("Deleting nav file...");
                File.Delete(navFile);
                Console.WriteLine("Done");
                return;
            }

            var files = File.Exists(vmfFile) && File.Exists(navFile);

            Console.WriteLine($"vmf: {vmfFile} -> {(File.Exists(vmfFile) ? " Found" : " Not found")}");

            if (!files)
            {
                Console.WriteLine("One file wasn't found. Cancelling.");
                return;
            }

            Console.WriteLine("Parsing nav...");
            NavFile nav = new NavFile(navFile);

            Console.WriteLine("Parsing vmf for places...\n");
            VmfParser p = new VmfParser(vmfFile);
            
            // insert places
            nav.Places = new PlaceName[p.VisGroups.Count];
            nav.PlaceCount = (ushort)p.VisGroups.Count;
            
            Console.WriteLine($"{nav.PlaceCount} places found:");
            for(var i = 0; i < nav.PlaceCount; i++)
            {
                nav.Places[i].Name = p.VisGroups[i].Name.Substring(3) + "\0";
                nav.Places[i].Length = (ushort)nav.Places[i].Name.Length;
                Console.WriteLine($"\t{nav.Places[i].Name}");
            }

            // filter all solids that have ap_ visgroup. note: if solid has more visgroups assigned, only first one is considered.
            var apSolids = p.Solids.FindAll(s => p.VisGroups.Select(v => v.VisGroupId).Contains(s.VisGroupId));

            Console.WriteLine($"{apSolids.Count} solids with places found.");

            Console.WriteLine("Applying places...");
            for(var i = 0; i < nav.Areas.Length; i++)
            {
                foreach (var solid in apSolids)
                {
                    if (nav.Areas[i].GetAABB().CollidesWith(solid.AABB))
                    {
                        // PIHVI
                        // +1 because placename 0 is "no name"
                        var id = p.VisGroups.FindIndex(v => v.VisGroupId == solid.VisGroupId) + 1;
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
            Console.WriteLine("\nSaving nav file...");
            nav.SaveToFile(navFile);
            Console.WriteLine("Done!");
        }
    }
}
