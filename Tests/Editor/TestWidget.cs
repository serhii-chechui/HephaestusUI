using System;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem.Tests
{
    /// <summary>
    /// Lightweight <see cref="IWidget"/> test double backed by a real <see cref="Transform"/>,
    /// so it can be parented by <see cref="UILayer"/> and raise lifecycle events on demand.
    /// </summary>
    public class TestWidget : MonoBehaviour, IWidget
    {
        public event Action<IWidget> OnCreated;
        public event Action<IWidget> OnActivated;
        public event Action<IWidget> OnDeactivated;
        public event Action<IWidget> OnDismissed;

        public Transform Transform => transform;

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
