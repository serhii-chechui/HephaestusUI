using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem.Editor
{
    public class WidgetModelCreator
    {
        public void CreateWidgetModel(string widgetName)
        {
            var stringBuilder = new StringBuilder();
            
            // Using block
            stringBuilder.Append("using WTFGames.Hephaestus.UISystem;\n\n");
            
            // Namespace begins
            stringBuilder.Append($"namespace {Application.productName}.UI.{widgetName}\n");
            stringBuilder.Append("{\n");
            
            // Class begins
            stringBuilder.Append($"\tpublic class {widgetName}WidgetModel : WidgetData\n");
            stringBuilder.Append("\t{\n");
            
            // Class inner content
            stringBuilder.Append("\t\n");
            
            // Class ends
            stringBuilder.Append("\t}\n");
            
            // Namespace ends
            stringBuilder.Append("}");

            var scriptsFolderPath = Path.Combine(WidgetsAssistantConstants.AssetsFolderName, Application.productName, "UI", widgetName, WidgetsAssistantConstants.FolderScriptsName);

            if (!AssetDatabase.IsValidFolder(scriptsFolderPath))
            {
                AssetDatabase.CreateFolder(scriptsFolderPath, widgetName);
                AssetDatabase.Refresh();
            }

            File.WriteAllText(Path.Combine(scriptsFolderPath, $"{widgetName}WindowModel.cs"), stringBuilder.ToString());
        }
    }
}