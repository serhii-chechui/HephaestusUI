using System;
using System.Collections.Generic;
using System.Linq;
using HephaestusMobile.UISystem.Configs;
using HephaestusMobile.UISystem.Layer;
using HephaestusMobile.UISystem.WidgetController;
using UnityEngine;
using UnityEngine.EventSystems;
using HephaestusMobile.UISystem.WidgetsLibrary;
using HephaestusMobile.UISystem.WidgetView;
#if USE_URP
using UnityEngine.Rendering.Universal;
#endif
using UnityEngine.UI;

namespace Handler
{
    public class UIManagerHandler : MonoBehaviour
    {
        #region Public Variables
        public Camera UiCamera { get; private set; }
        
        #endregion
        
        private UIManagerConfig _uiManagerConfig;
        private WidgetsLibrary _widgetLibrary;
        private WidgetFactory _widgetFactory;
        
        private Canvas _canvas;
        private CanvasScaler _canvasScaler;
        private GraphicRaycaster _graphicRaycaster;
        
        private List<UILayer> _uiLayers;
        
        public void Initialize(UIManagerConfig uiManagerConfig, WidgetFactory widgetFactory)
        {
            _uiManagerConfig = uiManagerConfig;

            _widgetFactory = widgetFactory;
            
            gameObject.layer = LayerMask.NameToLayer("UI");
            
            if(_widgetLibrary == null) {
                _widgetLibrary = uiManagerConfig.widgetsLibrary;
            }

            // Add Canvas component
            if(_canvas == null) {
                _canvas = gameObject.AddComponent<Canvas>();
            }
            
            _canvas.worldCamera = UiCamera;
            
            _canvas.renderMode = uiManagerConfig.renderMode;
            _canvas.planeDistance = uiManagerConfig.planeDistance;

            // Add CanvasScaler
            if(_canvasScaler == null) {
                _canvasScaler = gameObject.AddComponent<CanvasScaler>();
            }
            
            // Add raycaster
            if (_graphicRaycaster == null)
            {
                _graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
            }

            _canvasScaler.uiScaleMode = uiManagerConfig.canvasScaleMode;
            _canvasScaler.screenMatchMode = uiManagerConfig.canvasScreenMatchMode;
            _canvasScaler.referenceResolution = new Vector2(uiManagerConfig.ReferenceResolution.x, uiManagerConfig.ReferenceResolution.y);
            _canvasScaler.matchWidthOrHeight = uiManagerConfig.matchWidthOrHeight;

            CreateUILayers(uiManagerConfig);

            if (UiCamera != null || !uiManagerConfig.createUICamera) return;
            
            var uiCameraGo = new GameObject("UICamera");
            uiCameraGo.transform.position = new Vector3(0, 0, -10);
            DontDestroyOnLoad(uiCameraGo);

            UiCamera                  = uiCameraGo.AddComponent<Camera>();
            UiCamera.cullingMask      = 1 << LayerMask.NameToLayer("UI");
            UiCamera.orthographic     = uiManagerConfig.orthographic;
            UiCamera.orthographicSize = uiManagerConfig.orthographicSize;
            UiCamera.clearFlags       = uiManagerConfig.cameraClearFlags;
            UiCamera.backgroundColor  = Color.grey;
            UiCamera.allowHDR         = false;
            UiCamera.allowMSAA        = false;
            
            #if USE_URP
            UiCamera.GetUniversalAdditionalCameraData().renderType = uiManagerConfig.cameraRenderType;
            #endif

            if (uiManagerConfig.createAudioListener) {
                uiCameraGo.AddComponent<AudioListener>();
            }
            
            DontDestroyOnLoad(gameObject);
            
            var eventsSystem = new GameObject("EvensSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            DontDestroyOnLoad(eventsSystem.gameObject);
        }
        
        #region Private Methods

        private void CreateUILayers(UIManagerConfig uiManagerConfig) {

            _uiLayers = new List<UILayer>();

            for(var i = 0; i < uiManagerConfig.uiLayersList.Count; i++) {
                var layerName = $"UI-Layer-{i}-{uiManagerConfig.uiLayersList[i]}";
                var uiLayer = new GameObject(layerName).AddComponent<UILayer>();
                uiLayer.transform.SetParent(transform, false);
                uiLayer.Init(this, i, renderMode: uiManagerConfig.renderMode);
                _uiLayers.Add(uiLayer);
            }
        }
        
        /// <summary>
        /// Returns UILayers list.
        /// </summary>
        /// <returns>List of UILayers.</returns>
        private List<UILayer> GetUiLayers() {
            return _uiLayers;
        }
        
        /// <summary>
        /// Returns UILayer buy ID.
        /// </summary>
        /// <param name="layerId">UILayer ID.</param>
        /// <returns>UILayer object.</returns>
        public UILayer GetUiLayerById(int layerId) {
            return _uiLayers[layerId];
        }

        /// <summary>
        /// Returns UILayer buy name.
        /// </summary>
        /// <param name="layerName">UILayer name.</param>
        /// <returns></returns>
        public UILayer GetUiLayerByName(string layerName) {
            return _uiLayers.Find(l => l.name == layerName);
        }
        
        #endregion

        public IWidget CreateUiWidgetWithData(string widgetType, object data, bool animate, bool allowDuplicates)
        {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets Duplicates
            if(layer.IsWidgetTypeAlreadyExists(widgetType) && !allowDuplicates) {
                Debug.LogWarning($"Widget of type {widgetType}, already exists at: {layer.name}");
                return null;
            }
            
            var widgetGuid = Guid.NewGuid().ToString();

            //Instantiate new Widget Prefab
            // var widgetPrefab = Instantiate(_widgetLibrary.GetPrefabByType(widgetType));
            // widgetPrefab.name = $"{widgetType.ToLowerInvariant()}-{widgetGuid}";

            var widget = _widgetFactory.Create(_widgetLibrary.GetPrefabByType(widgetType));

            //Register it in UILayer
            layer.RegisterWidget(allowDuplicates ? widgetGuid : widgetType, widget);

            var controller = widget.Transform.GetComponent<IWidgetControllerWithData>();
            controller.Initialize(widget, data);

            widget.Create();
            widget.Activate(animate);

            return widget;
        }

        public void ActivateWidgetByType(string widgetType, bool animated)
        {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets exists in layer
            if(!layer.IsWidgetTypeAlreadyExists(widgetType)) {
                return;
            }

            layer.GetWidgetByType(widgetType).Activate(animated);
        }

        public void DeactivateWidgetByType(string widgetType, bool animated)
        {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets exists in layer
            if(!layer.IsWidgetTypeAlreadyExists(widgetType)) {
                return;
            }

            layer.GetWidgetByType(widgetType).Deactivate(animated);
        }

        public void DismissWidgetByType(string widgetType)
        {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets exists in layer
            if(!layer.IsWidgetTypeAlreadyExists(widgetType)) {
                return;
            }

            layer.GetWidgetByType(widgetType).Dismiss();
        }

        public void DismissAllWidgets()
        { 
            foreach (var widget in _uiLayers.SelectMany(layer => layer.GetAllWidgetsInLayer())) {
                widget.Dismiss();
            }
        }

        public void DismissWidgetsInLayer(int layerIndex)
        { 
            var widgetsCount = _uiLayers[layerIndex].GetWidgetsCount();
            
            if(_uiLayers[layerIndex] != null && widgetsCount > 0) {
                _uiLayers[layerIndex].GetLastWidget().Dismiss();
            }
        }
    }
}