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
            var prefab = widgetLinks.Find(w => w.WidgetType == Convert.ToInt32(widgetType)).WidgetPrefab;
            return prefab;
        }

        public IWidget GetWidgetByType(Enum widgetType)
        {
            var widget = widgetLinks.Find(w => w.WidgetType == Convert.ToInt32(widgetType)).WidgetPrefab
                .GetComponent<IWidget>();
            return widget;
        }

        public int GetLayerByType(Enum widgetType)
        {
            var layer = widgetLinks.Find(w => w.WidgetType == Convert.ToInt32(widgetType)).WidgetLayer;
            return layer;
        }
    }
}