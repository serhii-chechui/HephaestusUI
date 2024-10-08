using UnityEngine;
using Zenject;

namespace HephaestusMobile.UISystem.WidgetView
{
    public class CustomWidgetFactory : IFactory<GameObject, IWidget>
    {
        private readonly DiContainer _container;

        public CustomWidgetFactory(DiContainer container)
        {
            _container = container;
        }

        public IWidget Create(GameObject prefab)
        {
            return _container.InstantiatePrefabForComponent<IWidget>(prefab);
        }
    }
}