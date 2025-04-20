using UnityEngine;
using Zenject;

namespace WTFGames.Hephaestus.UISystem
{
    public class HephaestusUIManagerInstaller : Installer<HephaestusUIManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<GameObject, IWidget, WidgetFactory>().FromFactory<CustomWidgetFactory>();
            Container.BindInterfacesTo<UIManager>().AsSingle();
        }
    }
}