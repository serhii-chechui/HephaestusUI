using System;
using System.Collections;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseUIWidget : MonoBehaviour, IWidget
    {
        [SerializeField]
        protected CanvasGroup canvasGroup;

        public event Action<IWidget> OnCreated;
        public event Action<IWidget> OnActivated;
        public event Action<IWidget> OnDeactivated;
        public event Action<IWidget> OnDismissed;

        public Transform Transform => transform;

        public void Create()
        {
            NotifyOnCreated();
        }

        public virtual void Activate(bool animated)
        {
            NotifyOnActivated();

            canvasGroup = GetComponent<CanvasGroup>();

            if (!animated)
            {
                gameObject.SetActive(true);
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                StartCoroutine(FadeIn_co());
            }
        }

        public virtual void Deactivate(bool animated)
        {
            NotifyOnDeactivated();

            canvasGroup = GetComponent<CanvasGroup>();

            if (!animated)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                StartCoroutine(FadeOut_co());
            }
        }

        public virtual void Dismiss()
        {
            NotifyOnDismissed();
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        public void NotifyOnCreated()
        {
            OnCreated?.Invoke(this);
        }

        public void NotifyOnActivated()
        {
            OnActivated?.Invoke(this);
        }

        public void NotifyOnDeactivated()
        {
            OnDeactivated?.Invoke(this);
        }

        public void NotifyOnDismissed()
        {
            OnDismissed?.Invoke(this);
        }

        private IEnumerator FadeIn_co()
        {
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.deltaTime;
                yield return null;
            }

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private IEnumerator FadeOut_co()
        {
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.deltaTime;
                yield return null;
            }

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}