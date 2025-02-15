using MasterServerToolkit.Networking;
using UnityEngine;

namespace MasterServerToolkit.Games
{
    public class ColorMessage : SerializablePacket
    {
        public Color Color { get; set; }

        public override void FromBinaryReader(EndianBinaryReader reader)
        {
            Color = new Color(
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle()
                );
        }

        public override void ToBinaryWriter(EndianBinaryWriter writer)
        {
            writer.Write(Color.r);
            writer.Write(Color.g);
            writer.Write(Color.b);
            writer.Write(Color.a);
        }
    }
}
