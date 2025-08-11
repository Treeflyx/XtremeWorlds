using Core;
using CSScriptLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Configurations;
using Core.Globals;
using static Core.Net.Packets;

namespace Client.Game.Objects
{
    public class Ui
    {
        public static dynamic? Instance { get; private set; }

        public static void Load()
        {
            // Load the script file
            var scriptPath = System.IO.Path.Combine(DataPath.Skins, SettingsManager.Instance.Skin + ".cs");
            if (File.Exists(scriptPath))
            {
                var lines = File.ReadLines(scriptPath, Encoding.UTF8).ToArray();
                if (lines.Length > 0)
                {
                    Data.Ui.Code = lines;
                }
                else
                {
                    Data.Ui.Code = Array.Empty<string>();
                }
            }
            else
            {
                Data.Ui.Code = Array.Empty<string>();
            }

            string code = (Data.Ui.Code != null && Data.Ui.Code.Length > 0) ? string.Join(Environment.NewLine, Data.Ui.Code) : string.Empty;

            try
            {
                // Use the Roslyn evaluator directly for dynamic code loading
                var evaluator = CSScript.RoslynEvaluator;
                CSScript.EvaluatorConfig.Engine = EvaluatorEngine.Roslyn;

                // Dynamically load and execute the script
                dynamic script = evaluator
                    .ReferenceDomainAssemblies()
                    .LoadCode(code);

                if (script != null)
                {
                    Instance = script;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
