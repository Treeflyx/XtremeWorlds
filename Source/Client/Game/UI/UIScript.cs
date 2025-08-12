using Core.Configurations;
using Core.Globals;
using CSScriptLib;

namespace Client.Game.UI;

public static class UIScript
{
    public static dynamic? Instance { get; private set; }

    public static void Load()
    {
        var path = Path.Combine(DataPath.Skins, SettingsManager.Instance.Skin + ".cs");
        if (!File.Exists(path))
        {
            return;
        }

        try
        {
            var code = File.ReadAllText(path);

            var evaluator = CSScript.RoslynEvaluator;

            CSScript.EvaluatorConfig.Engine = EvaluatorEngine.Roslyn;

            dynamic script = evaluator
                .ReferenceDomainAssemblies()
                .LoadCode(code);

            if (script is not null)
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