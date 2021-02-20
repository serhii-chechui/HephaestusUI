using HephaestusMobile.UISystem.WidgetsLibrary;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace HephaestusMobile.UISystem.Editor {
    
    [CustomEditor(typeof(WidgetsLibrary.WidgetsLibrary))]
    public class WidgetsLibraryEditor : UnityEditor.Editor {
        
        private ReorderableList _reorderableList;

        private WidgetsLibrary.WidgetsLibrary WidgetsLibrary => target as WidgetsLibrary.WidgetsLibrary;

        private void OnEnable() {
            
            if(WidgetsLibrary == null) return;

            _reorderableList = new ReorderableList(WidgetsLibrary.widgetLinks, typeof(string), true, true, true, true);

            // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
            // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
            // which is a UnityEngine.Object
            // reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

            // Add listeners to draw events
            _reorderableList.drawHeaderCallback  += DrawHeader;
            _reorderableList.drawElementCallback += DrawElement;

            _reorderableList.onAddCallback    += AddItem;
            _reorderableList.onRemoveCallback += RemoveItem;
        }

        private void OnDisable() {
            
            if(_reorderableList == null) return;
            
            // Make sure we don't get memory leaks etc.
            _reorderableList.drawHeaderCallback  -= DrawHeader;
            _reorderableList.drawElementCallback -= DrawElement;

            _reorderableList.onAddCallback    -= AddItem;
            _reorderableList.onRemoveCallback -= RemoveItem;
        }

        /// <summary>
        /// Draws the header of the list
        /// </summary>
        /// <param name="rect"></param>
        private void DrawHeader(Rect rect) {
            GUI.Label(rect, "Dependencies between UIWidget prefab and UILayer", EditorStyles.boldLabel);
        }

        /// <summary>
        /// Draws one element of the list (ListItemExample)
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="index"></param>
        /// <param name="active"></param>
        /// <param name="focused"></param>
        private void DrawElement(Rect rect, int index, bool active, bool focused) {
            
            var item = WidgetsLibrary.widgetLinks[index];

            EditorGUI.BeginChangeCheck();

            item.WidgetType = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width * 0.5f - 40f, EditorGUIUtility.singleLineHeight), item.WidgetType);
            item.WidgetPrefab = (GameObject) EditorGUI.ObjectField(new Rect(rect.x + rect.width * 0.5f - 32f, rect.y, rect.width * 0.5f - 32f, EditorGUIUtility.singleLineHeight), item.WidgetPrefab, typeof(GameObject), false); 
            item.WidgetLayer = EditorGUI.IntField(new Rect(rect.x + rect.width - 40f, rect.y, 32f, EditorGUIUtility.singleLineHeight), item.WidgetLayer);

            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(target);
            }

            // If you are using a custom PropertyDrawer, this is probably better
            // EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
            // Although it is probably smart to cach the list as a private variable ;)
        }

        private void AddItem(ReorderableList list) {
            WidgetsLibrary.widgetLinks.Add(new WidgetsLibraryData() {WidgetType = "NEW_UI_WIDGET", WidgetPrefab = null, WidgetLayer = 0});
            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list) {
            WidgetsLibrary.widgetLinks.RemoveAt(list.index);

            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();
            
            if(_reorderableList == null) return;

            // Actually draw the list in the inspector
            _reorderableList.DoLayoutList();

            EditorGUILayout.Space();

            if (GUILayout.Button("Save Library", GUILayout.ExpandWidth(true), GUILayout.Height(32f))) {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }       
    }
}