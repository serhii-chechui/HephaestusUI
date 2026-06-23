using System;
using System.Collections.Generic;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem
{
    [CreateAssetMenu(fileName = "WidgetsLibrary", menuName = "HephaestusMobile/Core/UI/WidgetsLibrary")]
    public class WidgetsLibrary : ScriptableObject
    {
        public WidgetsLibraryConstants widgetsLibraryConstants;

        [HideInInspector]
        public List<WidgetsLibraryData> widgetLinks = new List<WidgetsLibraryData>();

        public GameObject GetPrefabByType(Enum widgetType)
        {
            var link = FindLink(widgetType);
            if (link == null)
            {
                return null;
            }

            if (link.WidgetPrefab == null)
            {
                Debug.LogError($"WidgetsLibrary: prefab for widget type {widgetType} is not assigned.");
                return null;
            }

            return link.WidgetPrefab;
        }

        public IWidget GetWidgetByType(Enum widgetType)
        {
            var prefab = GetPrefabByType(widgetType);
            return prefab != null ? prefab.GetComponent<IWidget>() : null;
        }

        public int GetLayerByType(Enum widgetType)
        {
            var link = FindLink(widgetType);
            return link?.WidgetLayer ?? -1;
        }

        private WidgetsLibraryData FindLink(Enum widgetType)
        {
            var key = Convert.ToInt32(widgetType);
            var link = widgetLinks.Find(w => w.WidgetType == key);
            if (link == null)
            {
                Debug.LogError($"WidgetsLibrary: no entry registered for widget type {widgetType}.");
            }

            return link;
        }
    }
}