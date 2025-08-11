using Core;
using Client.Net;
using Core.Globals;
using Core.Net;

namespace Client;

public class Script
{
    public static string TempFile = System.IO.Path.GetTempFileName() + ".cs";

    public static void Packet_EditScript(ReadOnlyMemory<byte> data)
    {
        var packetReader = new PacketReader(data);

        var nextChunk = packetReader.ReadInt32();
        var lineOffset = packetReader.ReadInt32();
        var numberOfLinesTotal = packetReader.ReadInt32();
        var numberOfLinesReceived = packetReader.ReadInt32();

        Array.Resize(ref Data.Script.Code, numberOfLinesTotal);

        for (var i = 0; i < numberOfLinesReceived; i++)
        {
            Data.Script.Code[lineOffset + i] = packetReader.ReadString();
        }

        if (nextChunk != -1) /* Request the next chunk if there is more data... */
        {
            var packetWriter = new PacketWriter(8);

            packetWriter.WriteEnum(Packets.ClientPackets.CRequestEditScript);
            packetWriter.WriteInt32(nextChunk);

            Network.Send(packetWriter);

            return;
        }

        GameState.InitScriptEditor = true;
    }
}