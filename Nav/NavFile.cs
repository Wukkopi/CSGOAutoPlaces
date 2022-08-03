using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSGOAutoPlaces.Nav
{
    class NavFile
    {
        public uint MagicNumber;
        public uint Version;
        public uint SubVersion;
        public uint BspSize;
        public byte Analyzed;
        public ushort PlaceCount;
        public PlaceName[] Places; // length = count
        public byte HasUnnamedAreas;
        public uint AreaCount;
        public NavArea[] Areas; // length = count
        public uint LadderCount;
        public NavLadder[] Ladders; // length = count
        public byte[] CustomData; // length = eos

        public NavFile(string filePath)
        {
            using (var reader = new BinaryReader(new FileStream(filePath, FileMode.Open)))
            {
                DeSerialize(reader);
            }
        }

        public void SaveToFile(string filePath)
        {
            using (var writer = new BinaryWriter(new FileStream(filePath, FileMode.Create)))
            {
                Serialize(writer);
            } 
        }

        public void DeSerialize(BinaryReader reader)
        {
            MagicNumber = reader.ReadUInt32();
            Version = reader.ReadUInt32();
            SubVersion = reader.ReadUInt32();
            BspSize = reader.ReadUInt32();
            Analyzed = reader.ReadByte();
            
            PlaceCount = reader.ReadUInt16();
            Places = new PlaceName[PlaceCount];
            for(var i = 0; i < PlaceCount; i++)
            {
                Places[i].DeSerialize(reader);
            }

            HasUnnamedAreas = reader.ReadByte();

            AreaCount = reader.ReadUInt32();
            Areas = new NavArea[AreaCount];
            for (var i = 0; i < AreaCount; i++)
            {
                Areas[i].DeSerialize(reader);
            }

            LadderCount = reader.ReadUInt32();
            Ladders = new NavLadder[LadderCount];
            for (var i = 0; i < LadderCount; i++)
            {
                Ladders[i].DeSerialize(reader);
            }

            int eosLength = (int)(reader.BaseStream.Length - reader.BaseStream.Position);

            CustomData = reader.ReadBytes(eosLength);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MagicNumber);
            writer.Write(Version);
            writer.Write(SubVersion);
            writer.Write(BspSize);
            writer.Write(Analyzed);

            writer.Write(PlaceCount);
            foreach(var place in Places)
            {
                place.Serialize(writer);
            }

            writer.Write(HasUnnamedAreas);

            writer.Write(AreaCount);
            foreach(var area in Areas)
            {
                area.Serialize(writer);
            }

            writer.Write(LadderCount);

            foreach(var ladder in Ladders)
            {
                ladder.Serialize(writer);
            }
            writer.Write(CustomData);
        }
    }
}