using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct PlaceName
    {
        public ushort Length;
        public string Name;

        public void DeSerialize(BinaryReader reader)
        {
            Length = reader.ReadUInt16();
            Name = new string(reader.ReadChars(Length));
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Length);
            writer.Write(Name.ToCharArray());
        }
    }
}
