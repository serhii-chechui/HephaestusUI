using System;
using HephaestusMobile.UISystem.WidgetView;

namespace HephaestusMobile.UISystem.Manager {
    public interface IUIManager {
        
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
        IWidget CreateUiWidgetWithData(Enum widgetType, object data = null, bool animate = false, bool allowDuplicates = false);

        /// <summary>
        /// Activates widget by it's type.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <param name="animated">Should be animated.</param>
        void ActivateWidgetByType(Enum widgetType, bool animated = false);

        /// <summary>
        /// Deactivates widget by it's type.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <param name="animated">Should be animated.</param>
        void DeactivateWidgetByType(Enum widgetType, bool animated = false);

        /// <summary>
        /// Dismisses widget by it's type. 
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        void DismissWidgetByType(Enum widgetType);

        /// <summary>
        /// Dismiss All Widgets. Useful in case of logout or other application context operations. 
        /// </summary>
        void DismissAllWidgets();

        /// <summary>
        /// Dismiss All Widgets within the specific layer.
        /// </summary>
        /// <param name="layerIndex">Layer Index.</param>
        void DismissWidgetsInLayer(int layerIndex);
    }
}