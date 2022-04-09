using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct AreaIdArray
    {
        public uint Count;
        public uint[] AreaIds;

        public void DeSerialize(BinaryReader reader)
        {
            Count = reader.ReadUInt32();
            AreaIds = new uint[Count];
            for(var i = 0; i < Count; i++)
            {
                AreaIds[i] = reader.ReadUInt32();
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Count);
            foreach (var areaId in AreaIds)
            {
                writer.Write(areaId);
            }
        }
    }
}
