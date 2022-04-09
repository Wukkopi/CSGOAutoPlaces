using CSGOAutoPlaces.Nav;
using System;
using System.IO;

namespace CSGOAutoPlaces
{
    class Program
    {
        static void Main(string[] args)
        {
            NavFile file = new NavFile();

            using (var reader = new BinaryReader(new FileStream("./testfiles/de_ap_testmap.nav", FileMode.Open)))
            {
                file.DeSerialize(reader);
            }
        }
    }
}
