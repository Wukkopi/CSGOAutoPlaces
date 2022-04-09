using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct NavLadder
    {
        public enum Direction : uint
        {
            Up = 0,
            Down = 1,
            Count = 2
        }
        public uint LadderId;
        public float Width;
        public Vector3 TopCorner;
        public Vector3 BottomCorner;
        public Direction LadderDirection;
        public uint TopForwardAreaId;
        public uint TopLeftAreaId;
        public uint TopRightAreaId;
        public uint TopBehindAreaId;
        public uint BottomAreaId;
        public void DeSerialize(BinaryReader reader)
        {
            LadderId = reader.ReadUInt32();
            Width = reader.ReadSingle();

            TopCorner.X = reader.ReadSingle();
            TopCorner.Y = reader.ReadSingle();
            TopCorner.Z = reader.ReadSingle();
            
            BottomCorner.X = reader.ReadSingle();
            BottomCorner.Y = reader.ReadSingle();
            BottomCorner.Z = reader.ReadSingle();
            
            LadderDirection = (Direction)reader.ReadUInt32();
            TopForwardAreaId = reader.ReadUInt32();
            TopLeftAreaId = reader.ReadUInt32();
            TopRightAreaId = reader.ReadUInt32();
            TopBehindAreaId = reader.ReadUInt32();
            BottomAreaId = reader.ReadUInt32();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(LadderId);
            writer.Write(Width);

            writer.Write(TopCorner.X);
            writer.Write(TopCorner.Y);
            writer.Write(TopCorner.Z);

            writer.Write(BottomCorner.X);
            writer.Write(BottomCorner.Y);
            writer.Write(BottomCorner.Z);

            writer.Write((uint)LadderDirection);
            writer.Write(TopForwardAreaId);
            writer.Write(TopLeftAreaId);
            writer.Write(TopRightAreaId);
            writer.Write(TopBehindAreaId);
            writer.Write(BottomAreaId);
        }
    }
}
