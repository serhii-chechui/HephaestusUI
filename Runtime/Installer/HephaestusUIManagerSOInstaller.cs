using UnityEngine;
using Zenject;

namespace WTFGames.Hephaestus.UISystem
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