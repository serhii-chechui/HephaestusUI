using System;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem.Tests
{
    /// <summary>
    /// Lightweight <see cref="IWidget"/> test double. It is a plain class (not a MonoBehaviour)
    /// so it can live in the Editor-only test assembly, yet it owns a real <see cref="GameObject"/>
    /// so <see cref="UILayer"/> can parent it and it can raise lifecycle events on demand.
    /// </summary>
    public class TestWidget : IWidget
    {
        private readonly GameObject _gameObject;

        public TestWidget()
        {
            _gameObject = new GameObject("TestWidget");
        }

        public event Action<IWidget> OnCreated;
        public event Action<IWidget> OnActivated;
        public event Action<IWidget> OnDeactivated;
        public event Action<IWidget> OnDismissed;

        /// <summary>Backing GameObject, exposed so tests can destroy it during teardown.</summary>
        public GameObject GameObject => _gameObject;

        public Transform Transform => _gameObject.transform;

        public void Create() => NotifyOnCreated();
        public void Activate(bool animated) => NotifyOnActivated();
        public void Deactivate(bool animated) => NotifyOnDeactivated();
        public void Dismiss() => NotifyOnDismissed();

        public void NotifyOnCreated() => OnCreated?.Invoke(this);
        public void NotifyOnActivated() => OnActivated?.Invoke(this);
        public void NotifyOnDeactivated() => OnDeactivated?.Invoke(this);
        public void NotifyOnDismissed() => OnDismissed?.Invoke(this);
    }

    /// <summary>Widget type keys used across the UI system tests.</summary>
    public enum TestWidgetType
    {
        Menu = 0,
        Popup = 1,
        Hud = 2
    }
}
