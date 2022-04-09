using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct EncounterSpot
    {
        public uint AreaId;
        public byte ParametricDistance;
        public void DeSerialize(BinaryReader reader)
        {
            AreaId = reader.ReadUInt32();
            ParametricDistance = reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AreaId);
            writer.Write(ParametricDistance);
        }
    }
}
