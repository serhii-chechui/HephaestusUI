using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using HephaestusMobile.UISystem.Configs;
#if USE_NEWTONSOFT_JSON_3_2_1
using Newtonsoft.Json;
#endif
using UnityEditor.SceneManagement;
using UnityEditor.U2D;
using UnityEngine.EventSystems;
#if USE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using UnityEngine.U2D;
using UnityEngine.UI;

namespace WTFGames.Hephaestus.UI.Editor
{
    public class HephaestusWidgetsAssistant : EditorWindow
    {
        private string _widgetSetName;
        
        private bool _createAtlas;
        private bool _createScene;
        private bool _createScripts;

        private string _uiRootFolder;
        private string _widgetRootFolder;
        
        private const string FolderAtlasName = "Atlas";
        private const string FolderPrefabsName = "Prefabs";
        private const string FolderPreviewName = "Preview";
        private const string FolderScenesName = "Scenes";
        private const string FolderScriptsName = "Scripts";
        private const string FolderSpritesName = "Sprites";
        
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

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.LabelField("Asset Creation Options:");
            
            _createAtlas = EditorGUILayout.Toggle("Create Sprite Atlas", _createAtlas);
            _createScene = EditorGUILayout.Toggle("Create Preview Scene", _createScene);
            _createScripts = EditorGUILayout.Toggle("Create Scripts", _createScripts);
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(16);

            if (string.IsNullOrEmpty(_widgetSetName)) return;

            if (string.IsNullOrEmpty(_uiRootFolder))
            {
                _uiRootFolder = Path.Combine("Assets", Application.productName, "UI");
            }

            if(string.IsNullOrEmpty(_widgetSetName)) return;
            
            _widgetRootFolder = Path.Combine(_uiRootFolder, _widgetSetName);

            if (GUILayout.Button("Create Assets Set", GUILayout.ExpandWidth(true), GUILayout.Height(32)))
            {
                if (!AssetDatabase.IsValidFolder(_widgetRootFolder))
                {
                    AssetDatabase.CreateFolder(_uiRootFolder, _widgetSetName);
                    AssetDatabase.Refresh();
                }
                    
                if (_createAtlas)
                {
                    CreateFolder(FolderAtlasName);
                    CreateAtlas();
                }
                    
                CreateFolder(FolderPrefabsName);
                CreateFolder(FolderPreviewName);
                    
                if (_createScene)
                {
                    CreateFolder(FolderScenesName);
                    CreatePreviewScene();
                }

                if (_createScripts)
                {
                    CreateFolder(FolderScriptsName);

                    CreateAsmdef();
                    CreateModel();
                    CreateView();
                    CreateController();
                }
                    
                CreateFolder(FolderSpritesName);
            }
        }
        
        private void CreateFolder(string folderName)
        {
            var folder = Path.Combine(_widgetRootFolder, folderName);
            
            Debug.Log($"{folderName} Folder Path: {folder}");
            
            Debug.Log(AssetDatabase.IsValidFolder(folder));
            
            if (!AssetDatabase.IsValidFolder(folder))
            {
                AssetDatabase.CreateFolder(_widgetRootFolder, folderName);
                AssetDatabase.Refresh();
            }
        }

        private void CreateAtlas()
        {
            var atlasFolderPath = Path.Combine(_widgetRootFolder, "Atlas");
            
            Debug.Log($"{atlasFolderPath}");
            
            // Define the sprite atlas name
            // Change this to your desired sprite atlas name
            var atlasName = $"{_widgetSetName}";
            var atlasPath = $"{atlasFolderPath}/{atlasName}.spriteatlas";

            // Check if a Sprite Atlas with the same name already exists
            if (AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath) != null)
            {
                Debug.LogWarning($"A Sprite Atlas with the name '{atlasName}' already exists at {atlasPath}. Please choose a different name.");
                return;
            }

            // Create a new Sprite Atlas
            var spriteAtlas = new SpriteAtlas();

            // Set default settings (can be customized)
            var textureSettings = new SpriteAtlasTextureSettings
            {
                sRGB = true,
                filterMode = FilterMode.Bilinear,
                generateMipMaps = false,
            };
            
            spriteAtlas.SetTextureSettings(textureSettings);

            SpriteAtlasPackingSettings packingSettings = new SpriteAtlasPackingSettings
            {
                enableRotation = false,
                enableTightPacking = false,
                padding = 2,
            };
            
            spriteAtlas.SetPackingSettings(packingSettings);

            // Add selected textures to the Sprite Atlas
            // spriteAtlas.Add(selectedObjects);

            // Save the Sprite Atlas as an asset
            AssetDatabase.CreateAsset(spriteAtlas, atlasPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Sprite Atlas created at: {atlasPath}");
        }
        
        private void CreatePreviewScene()
        {
            // Specify the folder path where the scene will be saved
            // Change this to your desired folder path
            var folderPath = Path.Combine(_widgetRootFolder, "Scenes");

            // Ensure the folder exists
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogError($"The folder path '{folderPath}' does not exist. Please create the folder first.");
                return;
            }

            // Define the scene name
            var sceneName = $"{_widgetSetName}_Preview";
            var scenePath = $"{folderPath}/{sceneName}.unity";

            // Check if a scene with the same name already exists
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) != null)
            {
                Debug.LogError($"A scene with the name '{sceneName}' already exists at {scenePath}. Please choose a different name.");
                return;
            }

            // Create a new scene
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            
            // MainCamera
            var mainCamera = new GameObject("MainCamera");
            var cameraComponent = mainCamera.AddComponent<Camera>();
            cameraComponent.orthographic = true;
            cameraComponent.orthographicSize = 5;
            cameraComponent.clearFlags = CameraClearFlags.Color;
            cameraComponent.backgroundColor = Color.gray;
            
            // Canvas
            var canvasObjectGo = new GameObject("Canvas");

            var canvas = canvasObjectGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var uiConfigPath = Path.Combine("Assets/Hephaestus/UI/UIManagerConfig.asset");

            var uiConfig = AssetDatabase.LoadAssetAtPath<UIManagerConfig>(uiConfigPath);

            var referenceResolution = new Vector2(1920, 1080);

            if (uiConfig != null)
            {
                referenceResolution = uiConfig.ReferenceResolution;
            }
            
            var canvasScaler = canvasObjectGo.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = referenceResolution;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 1f;
            canvasScaler.referencePixelsPerUnit = 100f;
            
            var graphicRaycaster = canvasObjectGo.AddComponent<GraphicRaycaster>();
            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            graphicRaycaster.blockingMask = 1 << 1;
            
            // Event System
            var eventSystemGo = new GameObject("EventSystem");
            eventSystemGo.AddComponent<EventSystem>();
            
            #if USE_INPUT_SYSTEM
            eventSystemGo.AddComponent<InputSystemUIInputModule>();
            #else
            eventSystemGo.AddComponent<StandaloneInputModule>();
            #endif

            // Save the new scene to the specified path
            var saveResult = EditorSceneManager.SaveScene(newScene, scenePath);

            if (saveResult)
            {
                Debug.Log($"Scene created and saved at: {scenePath}");
            }
            else
            {
                Debug.LogError("Failed to save the scene. Please check the folder path and permissions.");
            }
        }

        private void CreateAsmdef()
        {
            // Specify the folder path where the .asmdef file will be saved
            var folderPath = Path.Combine("Assets", Application.productName, "UI", _widgetSetName, FolderScriptsName);

            // Ensure the folder exists
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogError($"The folder path '{folderPath}' does not exist. Please create the folder first.");
                return;
            }

            // Define the .asmdef file name
            var asmdefFileName = $"com.{Application.companyName}.{Application.productName}.ui.{_widgetSetName}".ToLower(); 
            var asmdefName = $"\"name\": \"{asmdefFileName}\"".ToLower();
            var asmdefPath = $"{folderPath}/{asmdefFileName}.asmdef";

            // Check if a .asmdef file with the same name already exists
            if (File.Exists(asmdefPath))
            {
                Debug.LogError($"A .asmdef file with the name '{asmdefFileName}' already exists at {asmdefPath}. Please choose a different name.");
                return;
            }
            
            var rootNamespace = $"{Application.companyName}.{Application.productName}.UI.{_widgetSetName}";

            // Create the .asmdef JSON content
            var asmdefContent = new
            {
                name = asmdefFileName,
                rootNamespace = rootNamespace,
                references = new[] { "Zenject", "Unity.TextMeshPro", "UnityEngine.UI", "com.wtfgames.hephaestus.ui" }, // Add references to other assemblies (e.g., Zenject)
                includePlatforms = Array.Empty<string>(), // Leave empty to include all platforms
                excludePlatforms = Array.Empty<string>(), // Leave empty to exclude no platforms
                allowUnsafeCode = false,
                overrideReferences = false,
                precompiledReferences = Array.Empty<string>(),
                autoReferenced = true,
                defineConstraints = Array.Empty<string>(),
                versionDefines = Array.Empty<object>(),
                noEngineReferences = false
            };

            // Convert the JSON content to a string
            #if USE_NEWTONSOFT_JSON_3_2_1
            var json = JsonConvert.SerializeObject(asmdefContent, Formatting.Indented);
            #else
            var json = JsonUtility.ToJson(asmdefContent, true);
            #endif

            // Save the .asmdef file
            File.WriteAllText(asmdefPath, json);
            AssetDatabase.Refresh();

            Debug.Log($".asmdef file created at: {asmdefPath}");
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

            var scriptsFolderPath = Path.Combine("Assets", Application.productName, "UI", _widgetSetName, FolderScriptsName);

            if (!AssetDatabase.IsValidFolder(scriptsFolderPath))
            {
                AssetDatabase.CreateFolder(scriptsFolderPath, _widgetSetName);
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

            var scriptsFolderPath = Path.Combine("Assets", Application.productName, "UI", _widgetSetName, FolderScriptsName);
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

            var scriptsFolderPath = Path.Combine("Assets", Application.productName, "UI", _widgetSetName, FolderScriptsName);
            File.WriteAllText(Path.Combine(scriptsFolderPath, $"{_widgetSetName}WidgetController.cs"), stringBuilder.ToString());
            
            AssetDatabase.Refresh();
        }
    }
}