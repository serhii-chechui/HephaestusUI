using System.Collections.Generic;
using System.Linq;
using HephaestusMobile.UISystem.Manager;
using HephaestusMobile.UISystem.WidgetView;
using UnityEngine;
using UnityEngine.UI;

namespace HephaestusMobile.UISystem.Layer {
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public class UILayer : MonoBehaviour {
        #region Private Variables

        private readonly Dictionary<string, IWidget> _widgets = new Dictionary<string, IWidget>();
        private RectTransform _rectTransform;
        private Canvas _canvas;

        #endregion

        #region Public Methods

        public void Init(UIManager uiManager, int order = 0, float planeDistance = 1f, RenderMode renderMode = RenderMode.ScreenSpaceOverlay) {
            gameObject.layer = LayerMask.NameToLayer("UI");

            if (_canvas == null) {
                _canvas = GetComponent<Canvas>();
            }

            _canvas.renderMode = renderMode;

            switch (renderMode) {
                case RenderMode.ScreenSpaceCamera:
                    _canvas.worldCamera = uiManager.UiCamera;
                    break;
                case RenderMode.WorldSpace:
                    _canvas.worldCamera = uiManager.UiCamera;
                    break;
            }

            if (order != 0) {
                _canvas.overrideSorting = true;
                _canvas.sortingOrder = order;
            }

            _canvas.planeDistance = planeDistance;

            if (_rectTransform == null) {
                _rectTransform = GetComponent<RectTransform>();
            }

            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.anchoredPosition = Vector2.one * 0.5f;

            _rectTransform.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// Register Widget in this UILayer.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <param name="widget">Widget object.</param>
        public void RegisterWidget(string widgetType, IWidget widget) {
            _widgets.Add(widgetType, widget);

            widget.Transform.SetParent(transform, false);

            widget.OnDismissed += OnWidgetDismissed;
            Debug.LogFormat("Registered: {0}", widget.Transform.name);
        }

        /// <summary>
        /// Returns Widget by it's name from WidgetsLibrary.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <returns>Widget object.</returns>
        public IWidget GetWidgetByType(string widgetType) {
            return _widgets[widgetType];
        }

        /// <summary>
        /// Returns the last Widget.
        /// </summary>
        /// <returns>Widget object.</returns>
        public IWidget GetLastWidget() {
            return _widgets.Values.Last();
        }

        /// <summary>
        /// Get widgets count inside current UILayer.
        /// </summary>
        /// <returns></returns>
        public int GetWidgetsCount() {
            return _widgets.Count;
        }

        /// <summary>
        /// Return the list of all widgets inside current layer.
        /// </summary>
        /// <returns></returns>
        public List<IWidget> GetAllWidgetsInLayer() {
            return _widgets.Values.ToList();
        }

        /// <summary>
        /// Check does widget already exists.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <returns>Widget object.</returns>
        public bool IsWidgetTypeAlreadyExists(string widgetType) {
            return _widgets.ContainsKey(widgetType);
        }

        #endregion

        #region Private Methods

        private void OnWidgetDismissed(IWidget widget) {
            var widgetType = _widgets.FirstOrDefault(x => x.Value == widget).Key;
            _widgets.Remove(widgetType);
            widget.OnDismissed -= OnWidgetDismissed;
        }

        #endregion
    }
}
