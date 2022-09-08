using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace Editor.ViewUtils
{
    public class HephaestusWidgetsAssistant : EditorWindow
    {
        private string _widgetSetName;

        private const string ScriptsStagesFolderPath = "Scripts/UI";
        
        [MenuItem("Hephaestus/Utilities/ViewsCreatorAssistant")]
        private static void ShowWindow()
        {
            var window = GetWindow<HephaestusWidgetsAssistant>();
            window.titleContent = new GUIContent("Widgets Creator Assistant");
            window.Show();
        }

        private void OnGUI()
        {
            _widgetSetName = EditorGUILayout.TextField("Widget Set Name", _widgetSetName);

            if (!string.IsNullOrEmpty(_widgetSetName))
            {
                if (GUILayout.Button("Create View Set", GUILayout.ExpandWidth(true), GUILayout.Height(32)))
                {
                    CreateModel();
                    CreateView();
                    CreateController();
                }
            }
        }

        private void CreateModel()
        {
            var stringBuilder = new StringBuilder();
            
            // Using block
            stringBuilder.Append("using HephaestusMobile.UISystem.WidgetData;\n\n");
            
            // Namespace begins
            stringBuilder.Append($"namespace {Application.productName}.UI.{_widgetSetName}\n");
            stringBuilder.Append("{\n");
            
            // Class begins
            stringBuilder.Append($"\tpublic class {_widgetSetName}WidgetModel : WidgetData\n");
            stringBuilder.Append("\t{\n");
            
            // Class inner content
            stringBuilder.Append("\t\n");
            
            // Class ends
            stringBuilder.Append("\t}\n");
            
            // Namespace ends
            stringBuilder.Append("}");

            var scriptsFolderPath = Path.Combine(Application.dataPath, Application.productName, ScriptsStagesFolderPath, _widgetSetName);

            if (!Directory.Exists(scriptsFolderPath))
            {
                AssetDatabase.CreateFolder(Path.Combine("Assets", Application.productName, ScriptsStagesFolderPath), _widgetSetName);
                AssetDatabase.Refresh();
            }

            File.WriteAllText(Path.Combine(scriptsFolderPath, $"{_widgetSetName}WindowModel.cs"), stringBuilder.ToString());
            
            AssetDatabase.Refresh();
        }
        
        private void CreateView()
        {
            var stringBuilder = new StringBuilder();
            
            // Using block
            stringBuilder.Append("using HephaestusMobile.UISystem.WidgetView;\n");
            stringBuilder.Append("using UnityEngine.UI;\n");
            stringBuilder.Append("using UnityEngine;\n\n");
            
            // Namespace begins
            stringBuilder.Append($"namespace {Application.productName}.UI.{_widgetSetName}\n");
            stringBuilder.Append("{\n");
            
            // Class begins
            stringBuilder.Append($"\tpublic class {_widgetSetName}Widget : BaseUIWidget\n");
            stringBuilder.Append("\t{\n");
            
            // Class inner content
            stringBuilder.Append("\t\tpublic void Initialize()\n");
            stringBuilder.Append("\t\t{\n\n");
            stringBuilder.Append("\t\t}\n");
            
            // Class ends
            stringBuilder.Append("\t}\n");
            
            // Namespace ends
            stringBuilder.Append("}");

            var scriptsFolderPath = Path.Combine(Application.dataPath, Application.productName, ScriptsStagesFolderPath, _widgetSetName);
            File.WriteAllText(Path.Combine(scriptsFolderPath, $"{_widgetSetName}Widget.cs"), stringBuilder.ToString());
            
            AssetDatabase.Refresh();
        }
        
        private void CreateController()
        {
            var stringBuilder = new StringBuilder();
            
            // Using block
            stringBuilder.Append("using HephaestusMobile.UISystem.WidgetController;\n");
            stringBuilder.Append("using HephaestusMobile.UISystem.WidgetView;\n");
            stringBuilder.Append("using Zenject;\n\n");
            
            // Namespace begins
            stringBuilder.Append($"namespace {Application.productName}.UI.{_widgetSetName}\n");
            stringBuilder.Append("{\n");
            
            // Class begins
            stringBuilder.Append($"\tpublic class {_widgetSetName}WidgetController : WidgetControllerWithData<{_widgetSetName}Widget, {_widgetSetName}WidgetModel>\n");
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

            var scriptsFolderPath = Path.Combine(Application.dataPath, Application.productName, ScriptsStagesFolderPath, _widgetSetName);
            File.WriteAllText(Path.Combine(scriptsFolderPath, $"{_widgetSetName}WidgetController.cs"), stringBuilder.ToString());
            
            AssetDatabase.Refresh();
        }
    }
}