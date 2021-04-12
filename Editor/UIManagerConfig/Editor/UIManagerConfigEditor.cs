using HephaestusMobile.UISystem.Configs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace HephaestusMobile.UISystem.Editor {
    [CustomEditor(typeof(UIManagerConfig))]
    public class UIManagerConfigEditor : UnityEditor.Editor {
    
        private GUIStyle _mainTitleStyle;

        public override VisualElement CreateInspectorGUI() {

            _mainTitleStyle = new GUIStyle {fontSize = 14, fontStyle = FontStyle.Bold, alignment = TextAnchor.UpperCenter, stretchWidth = true};

            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI() {

            var uiManagerConfig = (UIManagerConfig)target;
        
            EditorGUILayout.LabelField("UI Manager Config", _mainTitleStyle);
        
            EditorGUILayout.Space();
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        
            EditorGUILayout.LabelField("UI Layers:", EditorStyles.boldLabel);
        
            EditorGUILayout.Space();

            for (var i = 0; i < uiManagerConfig.uiLayersList.Count; i++) {
                uiManagerConfig.uiLayersList[i] = EditorGUILayout.TextField("Layer Name:", uiManagerConfig.uiLayersList[i]);
            }
        
            EditorGUILayout.Space();
        
            EditorGUILayout.BeginHorizontal();
        
            if (GUILayout.Button("Add Layer", GUILayout.Width(64), GUILayout.Height(24))) {
                uiManagerConfig.uiLayersList.Add("New UI Layer");
            }
        
            if (GUILayout.Button("Remove Layer", GUILayout.Width(86), GUILayout.Height(24))) {
                if (uiManagerConfig.uiLayersList.Count >= 1) {
                    uiManagerConfig.uiLayersList.RemoveAt(uiManagerConfig.uiLayersList.Count - 1);   
                }
            }
        
            EditorGUILayout.EndHorizontal();
        
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
        
            uiManagerConfig.widgetsLibrary = (WidgetsLibrary.WidgetsLibrary)EditorGUILayout.ObjectField("Widgets Library:", uiManagerConfig.widgetsLibrary, typeof(WidgetsLibrary.WidgetsLibrary),false);
        
            EditorGUILayout.Space();
        
            EditorGUILayout.LabelField("Canvas:", EditorStyles.boldLabel);

            uiManagerConfig.renderMode = (RenderMode)EditorGUILayout.EnumPopup("Canvas Render Mode:", uiManagerConfig.renderMode);
        
            EditorGUILayout.Space();
        
            EditorGUILayout.LabelField("Canvas Scaler:", EditorStyles.boldLabel);
        
            uiManagerConfig.canvasScaleMode = (CanvasScaler.ScaleMode)EditorGUILayout.EnumPopup("Canvas Scale Mode:", uiManagerConfig.canvasScaleMode);

            switch (uiManagerConfig.canvasScaleMode) {
        
                case CanvasScaler.ScaleMode.ConstantPixelSize:
                    uiManagerConfig.scaleFactor = EditorGUILayout.FloatField("Scale Factor:", uiManagerConfig.scaleFactor);
                    break;
            
                case CanvasScaler.ScaleMode.ScaleWithScreenSize:
                    uiManagerConfig.ReferenceResolution   = EditorGUILayout.Vector2IntField("Reference Resolution", uiManagerConfig.ReferenceResolution);
                    uiManagerConfig.canvasScreenMatchMode = (CanvasScaler.ScreenMatchMode)EditorGUILayout.EnumPopup("Canvas Scale Mode:", uiManagerConfig.canvasScreenMatchMode);
                    if (uiManagerConfig.canvasScreenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight) {
                        uiManagerConfig.matchWidthOrHeight = EditorGUILayout.Slider("Match: ", uiManagerConfig.matchWidthOrHeight, 0, 1f);
                    }
                    break;
            
                case CanvasScaler.ScaleMode.ConstantPhysicalSize:
                    uiManagerConfig.physicalUnits     = (CanvasScaler.Unit)EditorGUILayout.EnumPopup("Physical Units:", uiManagerConfig.physicalUnits);
                    uiManagerConfig.fallbackScreenDpi = EditorGUILayout.FloatField("Fallback Screen Dpi: ", uiManagerConfig.fallbackScreenDpi);
                    uiManagerConfig.fallbackSpriteDpi = EditorGUILayout.FloatField("Fallback Sprite Dpi: ", uiManagerConfig.fallbackSpriteDpi);
                    break;
            
            }
        
            uiManagerConfig.referencePixelPerUnit = EditorGUILayout.FloatField("Reference Pixel Per Unit:", uiManagerConfig.referencePixelPerUnit);
        
            EditorGUILayout.Space();
        
            EditorGUILayout.LabelField("UI Camera:", EditorStyles.boldLabel);

            uiManagerConfig.createUICamera   = EditorGUILayout.Toggle("Create UI Camera", uiManagerConfig.createUICamera);
            uiManagerConfig.createAudioListener = EditorGUILayout.Toggle("Create Audio Listener", uiManagerConfig.createAudioListener);
            uiManagerConfig.cameraDepth      = EditorGUILayout.IntField("UI Camera Depth:", uiManagerConfig.cameraDepth);
            uiManagerConfig.orthographic     = EditorGUILayout.Toggle("IS UI Camera Orthographic:", uiManagerConfig.orthographic);
            uiManagerConfig.orthographicSize = EditorGUILayout.FloatField("Orthographic Size:", uiManagerConfig.orthographicSize);
            uiManagerConfig.cameraClearFlags = (CameraClearFlags)EditorGUILayout.EnumPopup("Camera Clear Flags:", uiManagerConfig.cameraClearFlags);
            uiManagerConfig.backgroundColor  = EditorGUILayout.ColorField("Background Color:", uiManagerConfig.backgroundColor);
            uiManagerConfig.cameraRenderType = (CameraRenderType)EditorGUILayout.EnumPopup("Camera Render Type:", CameraRenderType.Base);
        
            EditorGUILayout.Space();

            if (GUILayout.Button("Save Config", GUILayout.ExpandWidth(true), GUILayout.Height(32))) {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }

        }
    }
}

