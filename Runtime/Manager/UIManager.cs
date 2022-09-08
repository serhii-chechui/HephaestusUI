using System;
using Handler;
using HephaestusMobile.UISystem.Configs;
using HephaestusMobile.UISystem.WidgetView;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HephaestusMobile.UISystem.Manager {
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
    public class UIManager : IInitializable, IDisposable, IUIManager {

        #region Private Variables

        private GameObject _preloaderPrefab;

        private UIManagerConfig _uiManagerConfig;

        private WidgetFactory _widgetFactory;
        
        private UIManagerHandler _uiManagerHandler;

        #endregion

        [Inject]
        public void Construct( UIManagerConfig uiManagerConfig, WidgetFactory widgetFactory) {
            _uiManagerConfig = uiManagerConfig;
            _widgetFactory = widgetFactory;
        }
        
        public void Initialize()
        {
            _uiManagerHandler = new GameObject("UIManagerHandler").AddComponent<UIManagerHandler>();
            _uiManagerHandler.Initialize(_uiManagerConfig, _widgetFactory);
        }

        public void Dispose()
        {
            
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
        public IWidget CreateUiWidgetWithData(string widgetType, object data = null, bool animate = false, bool allowDuplicates = false)
        {
            return _uiManagerHandler.CreateUiWidgetWithData(widgetType, data, animate, allowDuplicates);
        }

        /// <inheritdoc />
        public void ActivateWidgetByType(string widgetType, bool animated = false)
        {
            _uiManagerHandler.ActivateWidgetByType(widgetType, animated);
        }

        /// <inheritdoc />
        public void DeactivateWidgetByType(string widgetType, bool animated = false)
        {
            _uiManagerHandler.DeactivateWidgetByType(widgetType, animated);
        }
        
        /// <inheritdoc />
        public void DismissWidgetByType(string widgetType)
        {
            _uiManagerHandler.DismissWidgetByType(widgetType);
        }

        /// <inheritdoc />
        public void DismissAllWidgets()
        {
            _uiManagerHandler.DismissAllWidgets();
        }

        /// <inheritdoc />
        public void DismissWidgetsInLayer(int layerIndex)
        {
            _uiManagerHandler.DismissWidgetsInLayer(layerIndex);
        }
        
        #endregion
    }

}