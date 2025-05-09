using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
#if USE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using UnityEngine.SceneManagement;
#if USE_URP
using UnityEngine.Rendering.Universal;
#endif
using UnityEngine.UI;

namespace WTFGames.Hephaestus.UISystem
{
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
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

        private EventSystem _eventSystem;

        public void Initialize(UIManagerConfig uiManagerConfig, WidgetFactory widgetFactory)
        {
            _uiManagerConfig = uiManagerConfig;

            _widgetFactory = widgetFactory;

            gameObject.layer = LayerMask.NameToLayer("UI");

            if (_widgetLibrary == null)
            {
                _widgetLibrary = _uiManagerConfig.widgetsLibrary;
            }

            // Add Canvas component
            if (_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
            }

            _canvas.renderMode = _uiManagerConfig.renderMode;
            _canvas.planeDistance = _uiManagerConfig.planeDistance;

            // Add CanvasScaler
            if (_canvasScaler == null)
            {
                _canvasScaler = GetComponent<CanvasScaler>();
            }

            // Add raycaster
            if (_graphicRaycaster == null)
            {
                _graphicRaycaster = GetComponent<GraphicRaycaster>();
            }

            _canvasScaler.uiScaleMode = _uiManagerConfig.canvasScaleMode;
            _canvasScaler.screenMatchMode = _uiManagerConfig.canvasScreenMatchMode;
            _canvasScaler.referenceResolution = new Vector2(_uiManagerConfig.ReferenceResolution.x, _uiManagerConfig.ReferenceResolution.y);
            _canvasScaler.matchWidthOrHeight = _uiManagerConfig.matchWidthOrHeight;

            CreateUILayers(_uiManagerConfig);

            if (UiCamera == null && _uiManagerConfig.createUICamera)
            {
                var uiCameraGo = new GameObject("UICamera");
                uiCameraGo.transform.position = new Vector3(0, 0, -10);

                UiCamera = uiCameraGo.AddComponent<Camera>();
                UiCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
                UiCamera.orthographic = _uiManagerConfig.orthographic;
                UiCamera.orthographicSize = _uiManagerConfig.orthographicSize;
                UiCamera.clearFlags = _uiManagerConfig.cameraClearFlags;
                UiCamera.backgroundColor = Color.grey;
                UiCamera.allowHDR = false;
                UiCamera.allowMSAA = false;
                UiCamera.rect = new Rect(0, 0, 1, 1);
                UiCamera.targetTexture = null;
                UiCamera.depth = 1;

                #if USE_URP
                UpdateCameraStack();
                #endif

                _canvas.worldCamera = UiCamera;

                if (_uiManagerConfig.createAudioListener)
                {
                    UiCamera.gameObject.AddComponent<AudioListener>();
                }
            }

            if (EventSystem.current == null)
            {
                var newEventsSystem = new GameObject("EventSystem", typeof(EventSystem));
                #if USE_INPUT_SYSTEM
                newEventsSystem.AddComponent<InputSystemUIInputModule>();
                #else
                newEventsSystem.AddComponent<StandaloneInputModule>();
                #endif
                _eventSystem = newEventsSystem.GetComponent<EventSystem>();
            }
            else
            {
                _eventSystem = EventSystem.current;
            }

            if (!_uiManagerConfig.sharedInstance) return;

            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(UiCamera.gameObject);
            DontDestroyOnLoad(_eventSystem.gameObject);

            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        public void Dismiss()
        {
            SceneManager.sceneLoaded -= SceneLoadedHandler;
        }

        #region Private Methods

        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadMode)
        {
            UpdateCameraStack();
        }

        private void UpdateCameraStack()
        {
            var uiCameraData = UiCamera.GetUniversalAdditionalCameraData();
            uiCameraData.renderType = _uiManagerConfig.cameraRenderType;
            uiCameraData.requiresColorOption = CameraOverrideOption.Off;
            uiCameraData.requiresDepthOption = CameraOverrideOption.Off;
                
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
            }
                
            if (mainCamera != null)
            {
                var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();
                if (!mainCameraData.cameraStack.Contains(UiCamera))
                {
                    mainCameraData.renderType = CameraRenderType.Base;
                    mainCameraData.cameraStack.Add(UiCamera);
                }
                else
                {
                    Debug.LogWarning("UI Camera already in camera stack.");
                }

                _canvas.worldCamera = UiCamera;
            }
            else
            {
                Debug.LogWarning($"Within {SceneManager.GetActiveScene().name} MainCamera not found for stacking UI overlay camera.");
            }
        }

        private void CreateUILayers(UIManagerConfig uiManagerConfig)
        {
            _uiLayers = new List<UILayer>();

            for (var i = 0; i < uiManagerConfig.uiLayersList.Count; i++)
            {
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
        private List<UILayer> GetUiLayers()
        {
            return _uiLayers;
        }

        /// <summary>
        /// Returns UILayer buy ID.
        /// </summary>
        /// <param name="layerId">UILayer ID.</param>
        /// <returns>UILayer object.</returns>
        public UILayer GetUiLayerById(int layerId)
        {
            return _uiLayers[layerId];
        }

        /// <summary>
        /// Returns UILayer buy name.
        /// </summary>
        /// <param name="layerName">UILayer name.</param>
        /// <returns></returns>
        public UILayer GetUiLayerByName(string layerName)
        {
            return _uiLayers.Find(l => l.name == layerName);
        }

        #endregion

        public IWidget CreateUiWidgetWithData(Enum widgetType, object data, bool animate, bool allowDuplicates)
        {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets Duplicates
            if (layer.IsWidgetTypeAlreadyExists(widgetType) && !allowDuplicates)
            {
                Debug.LogWarning($"Widget of type {widgetType}, already exists at: {layer.name}");
                return null;
            }

            //Instantiate new Widget Prefab
            // var widgetPrefab = Instantiate(_widgetLibrary.GetPrefabByType(widgetType));
            // widgetPrefab.name = $"{widgetType.ToLowerInvariant()}-{widgetGuid}";

            var widget = _widgetFactory.Create(_widgetLibrary.GetPrefabByType(widgetType));

            //Register it in UILayer
            layer.RegisterWidget(widgetType, widget);

            var controller = widget.Transform.GetComponent<IWidgetControllerWithData>();
            controller.Initialize(widget, data);

            widget.Create();
            widget.Activate(animate);

            return widget;
        }

        public void ActivateWidgetByType(Enum widgetType, bool animated)
        {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets exists in layer
            if (!layer.IsWidgetTypeAlreadyExists(widgetType))
            {
                return;
            }

            layer.GetWidgetByType(widgetType).Activate(animated);
        }

        public void DeactivateWidgetByType(Enum widgetType, bool animated)
        {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets exists in layer
            if (!layer.IsWidgetTypeAlreadyExists(widgetType))
            {
                return;
            }

            layer.GetWidgetByType(widgetType).Deactivate(animated);
        }

        public void DismissWidgetByType(Enum widgetType)
        {
            //Find related UILayer
            var layer = _uiLayers[_widgetLibrary.GetLayerByType(widgetType)];

            //Check for UI Widgets exists in layer
            if (!layer.IsWidgetTypeAlreadyExists(widgetType))
            {
                return;
            }

            var widget = layer.GetWidgetByType(widgetType);
            if (widget == null)
            {
                Debug.LogWarning($"Widget of type {widgetType} is null.");
            }
            else
            {
                widget.Dismiss();
            }
        }

        public void DismissAllWidgets()
        {
            foreach (var widget in _uiLayers.SelectMany(layer => layer.GetAllWidgetsInLayer()))
            {
                widget.Dismiss();
            }
        }

        public void DismissWidgetsInLayer(int layerIndex)
        {
            var widgetsCount = _uiLayers[layerIndex].GetWidgetsCount();

            if (_uiLayers[layerIndex] != null && widgetsCount > 0)
            {
                _uiLayers[layerIndex].GetLastWidget().Dismiss();
            }
        }
    }
}