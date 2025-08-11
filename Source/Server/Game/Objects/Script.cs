using System.Text;
using Core;
using Core.Globals;
using Core.Net;
using CSScriptLib;
using Microsoft.Extensions.Logging;
using Server.Game.Net;
using static Core.Net.Packets;
using static Core.Globals.Command;

namespace Server;

public static class Script
{
    private const int MaxScriptLinesPerChunk = 100;

    public static dynamic? Instance { get; private set; }

    public static void HandleRequestEditScript(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Owner)
        {
            return;
        }

        var user = IsEditorLocked(session.Id, EditorType.Script);
        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) ColorName.BrightRed);
            return;
        }

        Data.TempPlayer[session.Id].Editor = EditorType.Script;

        var lines = Data.Script.Code ?? [];

        var packetReader = new PacketReader(bytes);

        var requestedChunk = packetReader.ReadInt32();
        var numberOfChunks = (int) Math.Ceiling((double) lines.Length / MaxScriptLinesPerChunk);
        var offset = requestedChunk * MaxScriptLinesPerChunk;
        var chunkLines = lines.Skip(offset).Take(MaxScriptLinesPerChunk).ToArray();
        if (chunkLines.Length == 0)
        {
            return;
        }

        var packetWriter = new PacketWriter();

        packetWriter.WriteEnum(ServerPackets.SScriptEditor);
        packetWriter.WriteInt32(requestedChunk < numberOfChunks - 1 ? requestedChunk + 1 : -1);
        packetWriter.WriteInt32(offset);
        packetWriter.WriteInt32(lines.Length);
        packetWriter.WriteInt32(chunkLines.Length);

        foreach (var line in chunkLines)
        {
            packetWriter.WriteString(line);
        }

        session.Channel.Send(packetWriter.GetBytes());
    }

    public static void HandleSaveScript(GameSession session, ReadOnlyMemory<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Owner)
        {
            return;
        }

        var path = DataPath.Database;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = System.IO.Path.Combine(path, "Script.cs");

        var script = packetReader.ReadString();

        File.WriteAllText(path, script, Encoding.UTF8);

        _ = LoadScriptAsync(session.Id);
    }

    public static async Task LoadScriptAsync(int playerId)
    {
        var path = System.IO.Path.Combine(DataPath.Database, "Script.cs");
        if (File.Exists(path))
        {
            Data.Script.Code = await File.ReadAllLinesAsync(path, Encoding.UTF8);
        }
        else
        {
            Data.Script.Code = [];
        }

        var script = Data.Script.Code != null && Data.Script.Code.Length > 0
            ? string.Join(Environment.NewLine, Data.Script.Code)
            : string.Empty;

        if (string.IsNullOrWhiteSpace(script))
        {
            NetworkSend.PlayerMsg(playerId, "No script code found to compile.", (int) ColorName.BrightRed);

            General.Logger.LogWarning("No script code found to compile");
            return;
        }

        try
        {
            var evaluator = CSScript.RoslynEvaluator;

            CSScript.EvaluatorConfig.Engine = EvaluatorEngine.Roslyn;

            dynamic instance = evaluator
                .ReferenceDomainAssemblies()
                .LoadCode(script);

            if (instance is not null)
            {
                Instance = instance;

                if (playerId > 0)
                {
                    NetworkSend.PlayerMsg(playerId, "Script saved successfully!", (int) ColorName.Yellow);
                }
            }
        }
        catch (Exception ex)
        {
            if (playerId > 0)
            {
                NetworkSend.PlayerMsg(playerId, ex.Message, (int) ColorName.BrightRed);
            }

            General.Logger.LogError(ex, "[Script] Failed to load script");
        }
    }
}