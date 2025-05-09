using System;
using UnityEngine.Serialization;

namespace WTFGames.Hephaestus.UISystem.Editor
{
    [Serializable]
    public class AssemblyDefinitionData
    {
        public string name;
        public string rootNamespace;
        public string[] references = Array.Empty<string>();
        public string[] includePlatforms = Array.Empty<string>();
        public string[] excludePlatforms = Array.Empty<string>();
        public bool allowUnsafeCode = false;
        public bool overrideReferences = false;
        public string[] precompiledReferences;
        public bool autoReferenced = true;
        public string[] defineConstraints;
        public object[] versionDefines;
        public bool noEngineReferences;
    }
}