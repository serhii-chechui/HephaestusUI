using System;
using Handler;
using HephaestusMobile.UISystem.Configs;
using HephaestusMobile.UISystem.WidgetView;
using UnityEngine;
using Zenject;

namespace HephaestusMobile.UISystem.Manager
{
    public class UIManager : IInitializable, IDisposable, IUIManager
    {
        #region Private Variables

        private GameObject _preloaderPrefab;

        [Inject]
        private UIManagerConfig _uiManagerConfig;

        [Inject]
        private WidgetFactory _widgetFactory;

        private UIManagerHandler _uiManagerHandler;

        #endregion

        public void Initialize()
        {
            Debug.Log("UIManager.Initialize");
            _uiManagerHandler = new GameObject("UIManagerHandler").AddComponent<UIManagerHandler>();
            _uiManagerHandler.Initialize(_uiManagerConfig, _widgetFactory);
        }

        public void Dispose()
        {
        }

        #region Preloader

        public void ShowPreloader()
        {
            //if(_preloaderWidget != null) {
            //    _preloaderWidget.Activate(false);
            //    _preloaderWidget.SetLoadingProgress(0);
            //}
        }

        public void HidePreloader()
        {
            //if(_preloaderWidget != null) {
            //    _preloaderWidget.Dismiss(true);
            //}
        }

        public void SetPreloaderProgress(float progress)
        {
            //if(_preloaderWidget != null) {
            //    _preloaderWidget.SetLoadingProgress(progress);
            //}
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public IWidget CreateUiWidgetWithData(Enum widgetType, object data = null, bool animate = false,
            bool allowDuplicates = false)
        {
            return _uiManagerHandler.CreateUiWidgetWithData(widgetType, data, animate, allowDuplicates);
        }

        /// <inheritdoc />
        public void ActivateWidgetByType(Enum widgetType, bool animated = false)
        {
            _uiManagerHandler.ActivateWidgetByType(widgetType, animated);
        }

        /// <inheritdoc />
        public void DeactivateWidgetByType(Enum widgetType, bool animated = false)
        {
            _uiManagerHandler.DeactivateWidgetByType(widgetType, animated);
        }

        /// <inheritdoc />
        public void DismissWidgetByType(Enum widgetType)
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