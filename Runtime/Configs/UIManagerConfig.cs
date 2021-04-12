using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace HephaestusMobile.UISystem.Configs {
    
    [CreateAssetMenu(fileName = "UIManagerConfig", menuName = "HephaestusMobile/Core/UI/UIManagerConfig")]
    public class UIManagerConfig : ScriptableObject {
        
        public List<string> uiLayersList = new List<string>();

        public WidgetsLibrary.WidgetsLibrary widgetsLibrary;

        public RenderMode renderMode;
        public CanvasScaler.ScaleMode canvasScaleMode;

        //Values for ConstantPixelSize
        public float scaleFactor = 1;

        //Values for ScaleWithScreenSize
        public Vector2Int ReferenceResolution = new Vector2Int(800, 600);
        public CanvasScaler.ScreenMatchMode canvasScreenMatchMode;
        public float matchWidthOrHeight = 0.5f;

        //Values for ConstantPhysicalSize
        public CanvasScaler.Unit physicalUnits = CanvasScaler.Unit.Points;
        public float fallbackScreenDpi = 96;
        public float fallbackSpriteDpi = 96;

        public float referencePixelPerUnit = 100;

        //UI Camera
        public bool createUICamera;
        public bool createAudioListener;
        public int cameraDepth;
        public float planeDistance = 10;
        public bool orthographic;
        public float orthographicSize = 2.5f;
        public Color backgroundColor = Color.grey;
        public CameraClearFlags cameraClearFlags;
        
        //public
        public CameraRenderType cameraRenderType;
    }
}