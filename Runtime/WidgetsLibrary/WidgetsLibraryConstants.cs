using System.Collections.Generic;
using UnityEngine;

namespace HephaestusMobile.UISystem.WidgetsLibrary
{
    [CreateAssetMenu(fileName = "WidgetsLibraryConstants", menuName = "HephaestusMobile/Core/UI/WidgetsLibraryConstants", order = 1)]
    public class WidgetsLibraryConstants : ScriptableObject
    {
        [HideInInspector]
        public string enumsPath;
        
        [HideInInspector]
        public List<string> uiMapKeys = new List<string>();
    }
}