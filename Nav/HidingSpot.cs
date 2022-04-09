using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct HidingSpot
    {
        public uint AreaId;
        public Vector3 Position;
        public byte Flags;
        public void DeSerialize(BinaryReader reader)
        {
            AreaId = reader.ReadUInt32();
            Position.X = reader.ReadSingle();
            Position.Y = reader.ReadSingle();
            Position.Z = reader.ReadSingle();
            Flags = reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AreaId);
            writer.Write(Position.X);
            writer.Write(Position.Y);
            writer.Write(Position.Z);
            writer.Write(Flags);
        }
    }
}
