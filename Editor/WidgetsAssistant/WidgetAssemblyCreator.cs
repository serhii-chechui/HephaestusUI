using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem.Editor
{
    public class WidgetAssemblyCreator
    {
        public void CreateAssembly(string widgetName)
        {
            // Specify the folder path where the .asmdef file will be saved
            var folderPath = Path.Combine(WidgetsAssistantConstants.AssetsFolderName, Application.productName, "UI", widgetName, WidgetsAssistantConstants.FolderScriptsName);

            // Ensure the folder exists
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogError($"The folder path '{folderPath}' does not exist. Please create the folder first.");
                return;
            }

            // Define the .asmdef file name
            var newAssemblyDefinitionData = new AssemblyDefinitionData();
            var name = $"com.{Application.companyName}.{Application.productName}.ui.{widgetName}".ToLower();
            newAssemblyDefinitionData.name = name;
            var rootNamespace = $"{Application.companyName}.{Application.productName}.UI.{widgetName}";
            newAssemblyDefinitionData.rootNamespace = rootNamespace;
            newAssemblyDefinitionData.references = new[] { "Zenject", "Unity.TextMeshPro", "UnityEngine.UI", "com.wtfgames.hephaestus.ui" };
            newAssemblyDefinitionData.includePlatforms = Array.Empty<string>();
            newAssemblyDefinitionData.excludePlatforms = Array.Empty<string>();
            newAssemblyDefinitionData.allowUnsafeCode = false;
            newAssemblyDefinitionData.overrideReferences = false;
            newAssemblyDefinitionData.precompiledReferences = Array.Empty<string>();
            newAssemblyDefinitionData.autoReferenced = true;
            newAssemblyDefinitionData.defineConstraints = Array.Empty<string>();
            newAssemblyDefinitionData.versionDefines = Array.Empty<object>();
            newAssemblyDefinitionData.noEngineReferences = false;
            
            var asmdefPath = $"{folderPath}/{newAssemblyDefinitionData.name}.asmdef";

            // Check if a .asmdef file with the same name already exists
            if (File.Exists(asmdefPath))
            {
                Debug.LogError($"A .asmdef file with the name '{newAssemblyDefinitionData.name}' already exists at {asmdefPath}. Please choose a different name.");
                return;
            }

            // Convert the JSON content to a string
            #if USE_NEWTONSOFT_JSON
            var json = JsonConvert.SerializeObject(newAssemblyDefinitionData, Formatting.Indented);
            #else
            var json = JsonUtility.ToJson(newAssemblyDefinitionData, true);
            #endif

            // Save the .asmdef file
            File.WriteAllText(asmdefPath, json);
        }
    }
}