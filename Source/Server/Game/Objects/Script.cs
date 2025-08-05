using System.Text;
using Core;
using CSScriptLib;
using Microsoft.Extensions.Logging;
using Mirage.Sharp.Asfw;
using Mirage.Sharp.Asfw.IO;
using Server.Game.Net;
using Server.Net;
using static Core.Packets;
using static Core.Global.Command;
using Path = Core.Path;

namespace Server;

public static class Script
{
    public static dynamic? Instance { get; private set; }

    public static void HandleRequestEditScript(GameSession session, ReadOnlySpan<byte> bytes)
    {
        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Owner)
        {
            return;
        }

        var user = IsEditorLocked(session.Id, EditorType.Script);
        if (!string.IsNullOrEmpty(user))
        {
            NetworkSend.PlayerMsg(session.Id, "The game editor is locked and being used by " + user + ".", (int) Color.BrightRed);
            return;
        }

        Data.TempPlayer[session.Id].Editor = EditorType.Script;

        var codeLines = Data.Script.Code ?? [];

        var buffer = new ByteStream(4);

        buffer.WriteInt32(codeLines.Length);

        foreach (var line in codeLines)
        {
            buffer.WriteString(line ?? string.Empty);
        }

        var data = Compression.CompressBytes(buffer.ToArray());

        var packet = new PacketWriter();

        packet.WriteEnum(ServerPackets.SScriptEditor);
        packet.WriteRaw(data);

        session.Channel.Send(packet.GetBytes());
    }

    public static void HandleSaveScript(GameSession session, ReadOnlySpan<byte> bytes)
    {
        var packetReader = new PacketReader(bytes);

        if (GetPlayerAccess(session.Id) < (byte) AccessLevel.Owner)
        {
            return;
        }

        var path = Path.Database;
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
        var path = System.IO.Path.Combine(Path.Database, "Script.cs");
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
            NetworkSend.PlayerMsg(playerId, "No script code found to compile.", (int) Color.BrightRed);

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
                    NetworkSend.PlayerMsg(playerId, "Script saved successfully!", (int) Color.Yellow);
                }
            }
        }
        catch (Exception e)
        {
            if (playerId > 0)
            {
                NetworkSend.PlayerMsg(playerId, e.Message, (int) Color.BrightRed);
            }

            General.Logger.LogError(e, "[Script] Failed to load script");
        }
    }
}