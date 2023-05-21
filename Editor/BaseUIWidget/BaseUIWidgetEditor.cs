using HephaestusMobile.UISystem.WidgetView;
using UnityEditor;
using UnityEngine;

namespace HephaestusMobile.UISystem.Editor {
    [CustomEditor(typeof(BaseUIWidget), true)]
    public class BaseUIWidgetEditor : UnityEditor.Editor {

        private bool _animated;
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
            GUILayout.Space(16);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            _animated = EditorGUILayout.Toggle("Animated", _animated);
            
            GUILayout.Space(8);
            
            var baseUIWidget = (BaseUIWidget)target;
            
            if (GUILayout.Button("Create", GUILayout.ExpandWidth(true), GUILayout.Height(32))) {
                baseUIWidget.Create();
            }

            if (GUILayout.Button("Activate", GUILayout.ExpandWidth(true), GUILayout.Height(32))) {
                baseUIWidget.Activate(_animated);
            }
            
            if (GUILayout.Button("Deactivate", GUILayout.ExpandWidth(true), GUILayout.Height(32))) {
                baseUIWidget.Deactivate(_animated);
            }
            
            if (GUILayout.Button("Dismiss", GUILayout.ExpandWidth(true), GUILayout.Height(32))) {
                baseUIWidget.Dismiss();
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}
