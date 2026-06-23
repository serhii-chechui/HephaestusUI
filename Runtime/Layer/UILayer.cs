using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WTFGames.Hephaestus.UISystem
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public class UILayer : MonoBehaviour
    {
        #region Private Variables

        private readonly Dictionary<int, List<IWidget>> _widgets = new Dictionary<int, List<IWidget>>();
        private RectTransform _rectTransform;
        private Canvas _canvas;

        #endregion

        #region Public Methods

        public void Init(UIManagerHandler uiManagerHandler, int order = 0, float planeDistance = 1f,
            RenderMode renderMode = RenderMode.ScreenSpaceOverlay)
        {
            Debug.Log("UILayer.Init");

            gameObject.layer = LayerMask.NameToLayer("UI");

            if (_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
            }

            _canvas.renderMode = renderMode;

            switch (renderMode)
            {
                case RenderMode.ScreenSpaceCamera:
                    _canvas.worldCamera = uiManagerHandler.UiCamera;
                    break;
                case RenderMode.WorldSpace:
                    _canvas.worldCamera = uiManagerHandler.UiCamera;
                    break;
            }

            if (order != 0)
            {
                _canvas.overrideSorting = true;
                _canvas.sortingOrder = order;
            }

            _canvas.planeDistance = planeDistance;

            if (_rectTransform == null)
            {
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
        public void RegisterWidget(Enum widgetType, IWidget widget)
        {
            var key = Convert.ToInt32(widgetType);
            if (!_widgets.TryGetValue(key, out var widgetsOfType))
            {
                widgetsOfType = new List<IWidget>();
                _widgets.Add(key, widgetsOfType);
            }

            widgetsOfType.Add(widget);
            widget.Transform.SetParent(transform, false);
            widget.OnDismissed += OnWidgetDismissed;
        }

        /// <summary>
        /// Returns the most recently registered Widget of the given type, or null if none exist.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <returns>Widget object.</returns>
        public IWidget GetWidgetByType(Enum widgetType)
        {
            return _widgets.TryGetValue(Convert.ToInt32(widgetType), out var widgetsOfType) && widgetsOfType.Count > 0
                ? widgetsOfType[widgetsOfType.Count - 1]
                : null;
        }

        /// <summary>
        /// Returns the last registered Widget in the layer.
        /// </summary>
        /// <returns>Widget object.</returns>
        public IWidget GetLastWidget()
        {
            IWidget last = null;
            foreach (var widgetsOfType in _widgets.Values)
            {
                if (widgetsOfType.Count > 0)
                {
                    last = widgetsOfType[widgetsOfType.Count - 1];
                }
            }

            return last;
        }

        /// <summary>
        /// Get widgets count inside current UILayer.
        /// </summary>
        /// <returns></returns>
        public int GetWidgetsCount()
        {
            return _widgets.Values.Sum(widgetsOfType => widgetsOfType.Count);
        }

        /// <summary>
        /// Return the list of all widgets inside current layer.
        /// </summary>
        /// <returns></returns>
        public List<IWidget> GetAllWidgetsInLayer()
        {
            return _widgets.Values.SelectMany(widgetsOfType => widgetsOfType).ToList();
        }

        /// <summary>
        /// Check does widget already exists.
        /// </summary>
        /// <param name="widgetType">Widget name from WidgetsLibrary.</param>
        /// <returns>Widget object.</returns>
        public bool IsWidgetTypeAlreadyExists(Enum widgetType)
        {
            return _widgets.TryGetValue(Convert.ToInt32(widgetType), out var widgetsOfType) && widgetsOfType.Count > 0;
        }

        #endregion

        #region Private Methods

        private void OnWidgetDismissed(IWidget widget)
        {
            widget.OnDismissed -= OnWidgetDismissed;

            int? emptyKey = null;
            foreach (var pair in _widgets)
            {
                if (pair.Value.Remove(widget))
                {
                    if (pair.Value.Count == 0)
                    {
                        emptyKey = pair.Key;
                    }

                    break;
                }
            }

            if (emptyKey.HasValue)
            {
                _widgets.Remove(emptyKey.Value);
            }
        }

        #endregion
    }
}