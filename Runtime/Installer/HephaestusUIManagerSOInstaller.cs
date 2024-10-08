using HephaestusMobile.UISystem.Configs;
using UnityEngine;
using Zenject;

namespace HephaestusMobile.UISystem.Installers
{
    [CreateAssetMenu(fileName = "HephaestusUIManagerSOInstaller",
        menuName = "HephaestusMobile/Core/UI/HephaestusUIManagerSOInstaller")]
    public class HephaestusUIManagerSOInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private UIManagerConfig uiManagerConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(uiManagerConfig);
        }
    }
}