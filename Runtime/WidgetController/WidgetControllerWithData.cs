using HephaestusMobile.UISystem.WidgetView;
using UnityEngine;

namespace HephaestusMobile.UISystem.WidgetController {
    public abstract class WidgetControllerWithData<TWidget, TData> : MonoBehaviour, IWidgetControllerWithData where TWidget : IWidget where TData : WidgetData.WidgetData {
        protected TWidget Widget { get; private set; }
        protected TData WidgetData { get; private set; }

        public void Initialize(object widget, object data) {
            Widget = (TWidget) widget;
            WidgetData = (TData) data;

            Widget.OnCreated += OnWidgetCreated;
            Widget.OnActivated += OnWidgetActivated;
            Widget.OnDeactivated += OnWidgetDeactivated;
            Widget.OnDismissed += OnWidgetDismissed;
        }

        private void OnDestroy() {
            if (Widget == null) return;
            Widget.OnCreated -= OnWidgetCreated;
            Widget.OnActivated -= OnWidgetActivated;
            Widget.OnDeactivated -= OnWidgetDeactivated;
            Widget.OnDismissed -= OnWidgetDismissed;
        }

        protected abstract void OnWidgetCreated(IWidget widget);
        protected abstract void OnWidgetActivated(IWidget widget);
        protected abstract void OnWidgetDeactivated(IWidget widget);
        protected abstract void OnWidgetDismissed(IWidget widget);
    }
}
