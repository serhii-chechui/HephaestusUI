using System;
using System.Collections.Generic;
using System.Linq;
using HephaestusMobile.UISystem.Configs;
using HephaestusMobile.UISystem.Layer;
using HephaestusMobile.UISystem.WidgetController;
using HephaestusMobile.UISystem.WidgetView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace HephaestusMobile.UISystem.Manager {
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
    public class UIManager : MonoBehaviour, IUIManager {

        #region Public Variables
        public Camera UiCamera { get; private set; }

        #endregion
        
        #region Private Variables

        private WidgetsLibrary.WidgetsLibrary _widgetLibrary;
        private List<UILayer> _uiLayers;
        
        private Canvas _canvas;
        private CanvasScaler _canvasScaler;

        private GameObject _preloaderPrefab;

        private UIManagerConfig _uiManagerConfig;

        private WidgetFactory _widgetFactory;

        #endregion

        [Inject]
        public void Construct( UIManagerConfig uiManagerConfig, WidgetFactory widgetFactory) {
            _uiManagerConfig = uiManagerConfig;
            _widgetFactory = widgetFactory;
        }

        /// <inheritdoc />
        public void Initialize(UIManagerConfig uiManagerConfig) {

            var eventsSystem = new GameObject("EvensSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            DontDestroyOnLoad(eventsSystem.gameObject);
            
            gameObject.layer = LayerMask.NameToLayer("UI");

            if(_widgetLibrary == null) {
                _widgetLibrary = uiManagerConfig.widgetsLibrary;
            }

            if(_canvas == null) {
                _canvas = GetComponent<Canvas>();
            }
            _canvas.renderMode = uiManagerConfig.renderMode;
            _canvas.planeDistance = uiManagerConfig.planeDistance;

            if(_canvasScaler == null) {
                _canvasScaler = GetComponent<CanvasScaler>();
            }
            _canvasScaler = GetComponent<CanvasScaler>();

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

            if (uiManagerConfig.createAudioListener) {
                uiCameraGo.AddComponent<AudioListener>();
            }
        }

        private void Start() {
            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = UiCamera;
            
            Initialize(_uiManagerConfig);
        }

        #region Preloader
        public void ShowPreloader() {
            //if(_preloaderWidget != null) {
            //    _preloaderWidget.Activate(false);
            //    _preloaderWidget.SetLoadingProgress(0);
            //}
        }

        public void HidePreloader() {
            //if(_preloaderWidget != null) {
            //    _preloaderWidget.Dismiss(true);
            //}
        }

        public void SetPreloaderProgress(float progress) {
            //if(_preloaderWidget != null) {
            //    _preloaderWidget.SetLoadingProgress(progress);
            //}
        }
        #endregion

        #region Public Methods

        /// <inheritdoc />
        public IWidget CreateUiWidgetWithData(string widgetType, object data = null, bool animate = false, bool allowDuplicates = false) {

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

        /// <inheritdoc />
        public void ActivateWidgetByType(string widgetType, bool animated = false) {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets exists in layer
            if(!layer.IsWidgetTypeAlreadyExists(widgetType)) {
                return;
            }

            layer.GetWidgetByType(widgetType).Activate(animated);
        }

        /// <inheritdoc />
        public void DeactivateWidgetByType(string widgetType, bool animated = false) {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets exists in layer
            if(!layer.IsWidgetTypeAlreadyExists(widgetType)) {
                return;
            }

            layer.GetWidgetByType(widgetType).Deactivate(animated);
        }
        
        /// <inheritdoc />
        public void DismissWidgetByType(string widgetType) {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets exists in layer
            if(!layer.IsWidgetTypeAlreadyExists(widgetType)) {
                return;
            }

            layer.GetWidgetByType(widgetType).Dismiss();
        }

        /// <inheritdoc />
        public void DismissAllWidgets() {
            foreach (var widget in _uiLayers.SelectMany(layer => layer.GetAllWidgetsInLayer())) {
                widget.Dismiss();
            }
        }

        public UILayer GetUiLayerById(int layerId) {
            return _uiLayers[layerId];
        }

        public UILayer GetUiLayerByName(string layerName) {
            return _uiLayers.Find(l => l.name == layerName);
        }
        
        public List<UILayer> GetUiLayers() {
            return _uiLayers;
        }

        public void DismissWidgetsInLayer(int layerIndex) {
            var widgetsCount = _uiLayers[layerIndex].GetWidgetsCount();
            
            if(_uiLayers[layerIndex] != null && widgetsCount > 0) {
                _uiLayers[layerIndex].GetLastWidget().Dismiss();
            }
        }
        #endregion

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
        #endregion
    }

}