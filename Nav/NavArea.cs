using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct NavArea
    {
        public uint Id;
        public uint AttributeFlag;
        public Vector3 NorthWestCorner;
        public Vector3 SouthEastCorner;
        public float NorthEastCornerHeight;
        public float SouthWestCornerHeight;
        public AreaIdArray[] ConnectionData; // length = 4
        public byte HidingSpotCount;
        public HidingSpot[] HidingSpots; // length = count
        public uint EncounterPathCount;
        public EncounterPath[] EncounterPaths; // length = count
        public ushort PlaceId;
        public AreaIdArray[] LadderData; // length = 2
        public Vector2 EarliestOccupationTime;
        public Vector4 LightIntensity;
        public uint AreaBindCount;
        public AreaBind[] AreaBinds; // length = count
        public uint AreaIdVisInherit;
        public byte ApproachInfoCount; // removed in v15 --> sus
        public ApproachInfo[] ApproachInfos;

        public AABB GetAABB()
        {
            Vector3 min = new Vector3();
            Vector3 max = new Vector3();

            min.X = Math.Min(NorthWestCorner.X, SouthEastCorner.X);
            min.Y = Math.Min(NorthWestCorner.Y, SouthEastCorner.Y);
            min.Z = Math.Min(NorthWestCorner.Z, SouthEastCorner.Z);
            max.X = Math.Max(NorthWestCorner.X, SouthEastCorner.X);
            max.Y = Math.Max(NorthWestCorner.Y, SouthEastCorner.Y);
            max.Z = Math.Max(NorthWestCorner.Z, SouthEastCorner.Z);

            return new AABB() { Min = min, Max = max };
        }

        public void DeSerialize(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            AttributeFlag = reader.ReadUInt32();

            NorthWestCorner.X = reader.ReadSingle();
            NorthWestCorner.Y = reader.ReadSingle();
            NorthWestCorner.Z = reader.ReadSingle();

            SouthEastCorner.X = reader.ReadSingle();
            SouthEastCorner.Y = reader.ReadSingle();
            SouthEastCorner.Z = reader.ReadSingle();

            NorthEastCornerHeight = reader.ReadSingle();
            SouthWestCornerHeight = reader.ReadSingle();
            
            ConnectionData = new AreaIdArray[4];
            for (var i = 0; i < 4; i++) // length = 4 source: trust me
            {
                ConnectionData[i].DeSerialize(reader);
            }

            HidingSpotCount = reader.ReadByte();
            HidingSpots = new HidingSpot[HidingSpotCount];
            for (var i = 0; i < HidingSpotCount; i++)
            {
                HidingSpots[i].DeSerialize(reader);
            }

            EncounterPathCount = reader.ReadUInt32();
            EncounterPaths = new EncounterPath[EncounterPathCount];
            for (var i = 0; i < EncounterPathCount; i++)
            {
                EncounterPaths[i].DeSerialize(reader);
            }

            PlaceId = reader.ReadUInt16();

            LadderData = new AreaIdArray[2];
            for (var i = 0; i < 2; i++) // length = 2 source: trust me
            {
                LadderData[i].DeSerialize(reader);
            }
  
            EarliestOccupationTime.X = reader.ReadSingle();
            EarliestOccupationTime.Y = reader.ReadSingle();

            LightIntensity.X = reader.ReadSingle();
            LightIntensity.Y = reader.ReadSingle();
            LightIntensity.Z = reader.ReadSingle();
            LightIntensity.W = reader.ReadSingle();

            AreaBindCount = reader.ReadUInt32();
            AreaBinds = new AreaBind[AreaBindCount];
            for (var i = 0; i < AreaBindCount; i++)
            {
                AreaBinds[i].DeSerialize(reader);
            }

            AreaIdVisInherit = reader.ReadUInt32();

            ApproachInfoCount = reader.ReadByte(); // removed in v15? --> sus
            ApproachInfos = new ApproachInfo[ApproachInfoCount];
            for (var i = 0; i < ApproachInfoCount; i++)
            {
                ApproachInfos[i].DeSerialize(reader);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(AttributeFlag);

            writer.Write(NorthWestCorner.X);
            writer.Write(NorthWestCorner.Y);
            writer.Write(NorthWestCorner.Z);

            writer.Write(SouthEastCorner.X);
            writer.Write(SouthEastCorner.Y);
            writer.Write(SouthEastCorner.Z);

            writer.Write(NorthEastCornerHeight);
            writer.Write(SouthWestCornerHeight);

            foreach (var connectionData in ConnectionData)
            {
                connectionData.Serialize(writer);
            }

            writer.Write(HidingSpotCount);
            foreach (var hidingSpot in HidingSpots)
            {
                hidingSpot.Serialize(writer);
            }

            writer.Write(EncounterPathCount);
            foreach(var encounterPath in EncounterPaths)
            {
                encounterPath.Serialize(writer);
            }

            writer.Write(PlaceId);

            foreach(var ladderData in LadderData)
            {
                ladderData.Serialize(writer);
            }

            writer.Write(EarliestOccupationTime.X);
            writer.Write(EarliestOccupationTime.Y);

            writer.Write(LightIntensity.X);
            writer.Write(LightIntensity.Y);
            writer.Write(LightIntensity.Z);
            writer.Write(LightIntensity.W);

            writer.Write(AreaBindCount);
            foreach(var areaBind in AreaBinds)
            {
                areaBind.Serialize(writer);
            }

            writer.Write(AreaIdVisInherit);

            writer.Write(ApproachInfoCount);
            foreach (var approachInfo in ApproachInfos)
            {
                approachInfo.Serialize(writer);
            }
        }
    }
}
