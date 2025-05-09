using System.IO;
using System.Text;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem.Editor
{
    public class WidgetViewCreator
    {
        public void CreateView(string widgetName)
        {
            var stringBuilder = new StringBuilder();
            
            // Using block
            stringBuilder.Append("using WTFGames.Hephaestus.UISystem;\n");
            stringBuilder.Append("using UnityEngine.UI;\n");
            stringBuilder.Append("using UnityEngine;\n\n");
            
            // Namespace begins
            stringBuilder.Append($"namespace {Application.productName}.UI.{widgetName}\n");
            stringBuilder.Append("{\n");
            
            // Class begins
            stringBuilder.Append($"\tpublic class {widgetName}Widget : BaseUIWidget\n");
            stringBuilder.Append("\t{\n");
            
            // Class inner content
            stringBuilder.Append("\t\tpublic void Initialize()\n");
            stringBuilder.Append("\t\t{\n\n");
            stringBuilder.Append("\t\t}\n");
            
            // Class ends
            stringBuilder.Append("\t}\n");
            
            // Namespace ends
            stringBuilder.Append("}");

            var scriptsFolderPath = Path.Combine("Assets", Application.productName, "UI", widgetName, WidgetsAssistantConstants.FolderScriptsName);
            File.WriteAllText(Path.Combine(scriptsFolderPath, $"{widgetName}Widget.cs"), stringBuilder.ToString());
        }
    }
}