using System;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem
{
    public interface IWidget
    {
        #region Actions

        event Action<IWidget> OnCreated;
        event Action<IWidget> OnActivated;
        event Action<IWidget> OnDeactivated;
        event Action<IWidget> OnDismissed;

        #endregion

        Transform Transform { get; }

        void Create();
        void Activate(bool animated);
        void Deactivate(bool animated);
        void Dismiss();

        void NotifyOnCreated();
        void NotifyOnActivated();
        void NotifyOnDeactivated();
        void NotifyOnDismissed();
    }
}