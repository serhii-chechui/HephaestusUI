using System;
using Zenject;

namespace WTFGames.Hephaestus.UISystem
{
    public class UIManager : IInitializable, IDisposable, IUIManager
    {
        #region Private Variables

        [Inject]
        private UIManagerHandler _uiManagerHandler;

        #endregion

        public void Initialize()
        {
            _uiManagerHandler.Initialize();
        }

        public void Dispose()
        {
            _uiManagerHandler.Dismiss();
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