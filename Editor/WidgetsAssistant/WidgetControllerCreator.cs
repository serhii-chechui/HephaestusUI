using System.IO;
using System.Text;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem.Editor
{
    public class WidgetControllerCreator
    {
        public void CreateWidgetController(string widgetName)
        {
            var stringBuilder = new StringBuilder();
            
            // Using block
            stringBuilder.Append("using WTFGames.Hephaestus.UISystem;\n");
            stringBuilder.Append("using Zenject;\n\n");
            
            // Namespace begins
            stringBuilder.Append($"namespace {Application.productName}.UI.{widgetName}\n");
            stringBuilder.Append("{\n");
            
            // Class begins
            stringBuilder.Append($"\tpublic class {widgetName}WidgetController : WidgetControllerWithData<{widgetName}Widget, {widgetName}WidgetModel>\n");
            stringBuilder.Append("\t{\n");
            
            // Class inner content
            stringBuilder.Append($"\t\tprotected override void OnWidgetCreated(IWidget widget)\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t}\n\n");
            
            stringBuilder.Append($"\t\tprotected override void OnWidgetActivated(IWidget widget)\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t}\n\n");
            
            stringBuilder.Append($"\t\tprotected override void OnWidgetDeactivated(IWidget widget)\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t}\n\n");
            
            stringBuilder.Append($"\t\tprotected override void OnWidgetDismissed(IWidget widget)\n");
            stringBuilder.Append("\t\t{\n");
            stringBuilder.Append("\t\t}\n\n");

            // Class ends
            stringBuilder.Append("\t}\n");
            
            // Namespace ends
            stringBuilder.Append("}");

            var scriptsFolderPath = Path.Combine(WidgetsAssistantConstants.AssetsFolderName, Application.productName, "UI", widgetName, WidgetsAssistantConstants.FolderScriptsName);
            File.WriteAllText(Path.Combine(scriptsFolderPath, $"{widgetName}WidgetController.cs"), stringBuilder.ToString());
        }
    }
}