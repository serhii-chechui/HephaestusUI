using HephaestusMobile.UISystem.Manager;
using HephaestusMobile.UISystem.WidgetView;
using UnityEngine;
using Zenject;

namespace HephaestusMobile.UISystem.Installers
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