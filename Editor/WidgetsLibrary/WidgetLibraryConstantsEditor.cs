using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem.Editor
{
    [CustomEditor(typeof(WidgetsLibraryConstants))]
    public class WidgetLibraryConstantsEditor : UnityEditor.Editor
    {
        private const string EntityType = "UI";

        private StringBuilder _stringBuilder;

        private string _enumClassName = "WidgetsLibraryConstants";

        private string _newConstantKey = string.Empty;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var mapConstants = (WidgetsLibraryConstants)target;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Export:", EditorStyles.largeLabel);

            _enumClassName = EditorGUILayout.TextField("Enum Class Name:", _enumClassName);

            EditorGUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(mapConstants.enumsPath))
            {
                EditorGUILayout.LabelField("Path", mapConstants.enumsPath, GUILayout.ExpandWidth(true));
            }

            if (GUILayout.Button("Pick", GUILayout.Width(96)))
            {
                mapConstants.enumsPath = EditorUtility.OpenFolderPanel("Pick The Folder", "", "");
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Export to enum", GUILayout.ExpandWidth(true), GUILayout.Height(32)))
            {
                ExportKeysToEnum(mapConstants);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField($"Add New {EntityType} Keys:", EditorStyles.largeLabel);

            _newConstantKey = EditorGUILayout.TextField("New Key:", _newConstantKey).Replace(' ', '_').ToUpper();

            if (GUILayout.Button($"Add New {EntityType} Key", GUILayout.ExpandWidth(true), GUILayout.Height(32)))
            {
                mapConstants.uiMapKeys.Add(_newConstantKey);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField($"List of {EntityType} Keys:", EditorStyles.largeLabel);

            if (mapConstants.uiMapKeys == null) return;

            for (int i = 0; i < mapConstants.uiMapKeys.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                mapConstants.uiMapKeys[i] = EditorGUILayout.TextField(mapConstants.uiMapKeys[i]);

                if (GUILayout.Button("Remove"))
                {
                    mapConstants.uiMapKeys.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            if (GUILayout.Button("Save Config", GUILayout.ExpandWidth(true), GUILayout.Height(32)))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void ExportKeysToEnum(WidgetsLibraryConstants audioLibraryConstants)
        {
            _stringBuilder = new StringBuilder();

            _stringBuilder.Append($"public enum {_enumClassName} : byte\n");

            _stringBuilder.Append("{\n");

            for (var i = 0; i < audioLibraryConstants.uiMapKeys.Count; i++)
            {
                var coma = i < audioLibraryConstants.uiMapKeys.Count - 1 ? "," : string.Empty;
                _stringBuilder.Append($"\t{audioLibraryConstants.uiMapKeys[i].ToUpper()} = {i}{coma}\n");
            }

            _stringBuilder.Append("}");

            var filename = $"{_enumClassName}.cs";
            File.WriteAllText(Path.Combine(audioLibraryConstants.enumsPath, filename), _stringBuilder.ToString());

            AssetDatabase.Refresh();
        }
    }
}