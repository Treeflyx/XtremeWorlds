using Assimp;
using Core;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Mirage.Sharp.Asfw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Script
    {
        public static string TempFile = System.IO.Path.GetTempFileName() + ".cs";

        public static void Packet_EditScript(ReadOnlyMemory<byte> data)
        {
            ByteStream buffer;
            buffer = new ByteStream(Mirage.Sharp.Asfw.IO.Compression.DecompressBytes(data.ToArray()));

            int lineCount = buffer.ReadInt32();
            string[] lines = new string[lineCount];
            Array.Resize(ref Data.Script.Code, lineCount); 
            int line = 0;

            for (int i = 0; i < 256; i++)
            {
                line = buffer.ReadInt32();
                lines[line] = buffer.ReadString();
                Data.Script.Code[line] = lines[line];

                if (line == lineCount)
                {
                    break;
                }
            }

            if (line != lineCount)
            {
                NetworkSend.SendRequestEditScript(line + 1);
            }

            buffer.Dispose();

            GameState.InitScriptEditor = true;
        }

        public static void ScriptEditorInit()
        {

        }
    }
}
