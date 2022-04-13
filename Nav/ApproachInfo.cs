using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct ApproachInfo
    {
        public uint TargetAreaId;
        public uint ApproachPrev;
        public byte Type;
        public uint Next;
        public byte Method;
        public void DeSerialize(BinaryReader reader)
        {
            TargetAreaId = reader.ReadUInt32();
            ApproachPrev = reader.ReadUInt32();
            Type = reader.ReadByte();
            Next = reader.ReadUInt32();
            Method = reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TargetAreaId);
            writer.Write(ApproachPrev);
            writer.Write(Type);
            writer.Write(Next);
            writer.Write(Method);
        }
    }
}
