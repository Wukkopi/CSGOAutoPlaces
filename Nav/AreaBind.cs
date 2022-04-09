using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct AreaBind
    {
        public enum VisibilityType : byte
        {
            NotVisible = 1,
            PotentiallyVisible = 2,
            CompletelyVisible = 3,
        }
        public uint BoundAreaId;
        public VisibilityType Visibility;
        public void DeSerialize(BinaryReader reader)
        {
            BoundAreaId = reader.ReadUInt32();
            Visibility = (VisibilityType)reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BoundAreaId);
            writer.Write((byte)Visibility);
        }
    }
}
