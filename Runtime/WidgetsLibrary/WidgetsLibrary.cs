using System.Collections.Generic;
using HephaestusMobile.UISystem.WidgetView;
using UnityEngine;

namespace HephaestusMobile.UISystem.WidgetsLibrary {
    [CreateAssetMenu(fileName = "WidgetsLibrary", menuName = "HephaestusMobile/Core/UI/WidgetsLibrary")]
    public class WidgetsLibrary : ScriptableObject {
        
        [HideInInspector] public List<WidgetsLibraryData> widgetLinks = new List<WidgetsLibraryData>();

        public GameObject GetPrefabByType(string widgetType) {
            var prefab = widgetLinks.Find(w => w.WidgetType == widgetType).WidgetPrefab;
            return prefab;
        }

        public IWidget GetWidgetByType(string widgetType) {
            var widget = widgetLinks.Find(w => w.WidgetType == widgetType).WidgetPrefab.GetComponent<IWidget>();
            return widget;
        }

        public int GetLayerByType(string widgetType) {
            var layer = widgetLinks.Find(w => w.WidgetType == widgetType).WidgetLayer;
            return layer;
        }
    }
}