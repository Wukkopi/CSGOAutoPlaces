using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    struct EncounterPath
    {
        public enum Direction : byte
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }

        public uint EntryAreaId;
        public Direction EntryDirection;
        public uint DestinationAreaId;
        public Direction DestinationDirection;
        public byte EncounterSpotCount;
        public EncounterSpot[] EncounterSpots;
        public void DeSerialize(BinaryReader reader)
        {
            EntryAreaId = reader.ReadUInt32();
            EntryDirection = (Direction)reader.ReadByte();
            DestinationAreaId = reader.ReadUInt32();
            DestinationDirection = (Direction)reader.ReadByte();
            EncounterSpotCount = reader.ReadByte();
            EncounterSpots = new EncounterSpot[EncounterSpotCount];
            for(var i = 0; i < EncounterSpotCount; i++)
            {
                EncounterSpots[i].DeSerialize(reader);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(EntryAreaId);
            writer.Write((byte)EntryDirection);
            writer.Write(DestinationAreaId);
            writer.Write((byte)DestinationDirection);
            writer.Write(EncounterSpotCount);
            foreach(var encounterSpot in EncounterSpots)
            {
                encounterSpot.Serialize(writer);
            }
        }
    }
}
