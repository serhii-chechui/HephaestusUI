using System.Collections.Generic;
using HephaestusMobile.UISystem.Configs;
using HephaestusMobile.UISystem.Layer;
using HephaestusMobile.UISystem.WidgetView;

namespace HephaestusMobile.UISystem.Manager {
    public interface IUIManager {
        /// <summary>
        /// Initializes UIManager.
        /// </summary>
        /// <param name="uiManagerConfig">UIManager config.</param>
        void Initialize(UIManagerConfig uiManagerConfig);

        /// <summary>
        /// Shows the preloader.
        /// </summary>
        void ShowPreloader();

        /// <summary>
        /// Hides the preloader.
        /// </summary>
        void HidePreloader();

        /// <summary>
        /// Set preloader progress.
        /// </summary>
        /// <param name="progress">Preloader progress value.</param>
        void SetPreloaderProgress(float progress);

        /// <summary>
        /// Shows UIWidget with WidgetData.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <param name="data">WidgetData object.</param>
        /// <param name="animate">Should be widget animated.</param>
        /// <param name="allowDuplicates">Allows to create multiple widgets of the the same type.</param>
        IWidget CreateUiWidgetWithData(string widgetType, object data = null, bool animate = false, bool allowDuplicates = false);

        /// <summary>
        /// Activates widget by it's type.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <param name="animated">Should be animated.</param>
        void ActivateWidgetByType(string widgetType, bool animated = false);

        /// <summary>
        /// Deactivates widget by it's type.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <param name="animated">Should be animated.</param>
        void DeactivateWidgetByType(string widgetType, bool animated = false);

        /// <summary>
        /// Dismisses widget by it's type. 
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        void DismissWidgetByType(string widgetType);

        /// <summary>
        /// Dismiss All Widgets. Useful in case of logout or other application context operations. 
        /// </summary>
        void DismissAllWidgets();

        /// <summary>
        /// Returns UILayer buy ID.
        /// </summary>
        /// <param name="layerId">UILayer ID.</param>
        /// <returns>UILayer object.</returns>
        UILayer GetUiLayerById(int layerId);

        /// <summary>
        /// Returns UILayer buy name.
        /// </summary>
        /// <param name="layerName">UILayer name.</param>
        /// <returns></returns>
        UILayer GetUiLayerByName(string layerName);

        /// <summary>
        /// Returns UILayers list.
        /// </summary>
        /// <returns>List of UILayers.</returns>
        List<UILayer> GetUiLayers();
    }
}