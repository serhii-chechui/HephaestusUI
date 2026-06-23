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

        [SerializeField]
        [Tooltip("Duration of the fade in/out animation, in seconds.")]
        protected float fadeDuration = 1f;

        private Coroutine _fadeRoutine;

        public event Action<IWidget> OnCreated;
        public event Action<IWidget> OnActivated;
        public event Action<IWidget> OnDeactivated;
        public event Action<IWidget> OnDismissed;

        public Transform Transform => transform;

        protected virtual void Awake()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public void Create()
        {
            NotifyOnCreated();
        }

        public virtual void Activate(bool animated)
        {
            NotifyOnActivated();

            StopFade();

            if (!animated)
            {
                gameObject.SetActive(true);
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                // The GameObject must be active for the coroutine to run.
                gameObject.SetActive(true);
                _fadeRoutine = StartCoroutine(FadeIn_co());
            }
        }

        public virtual void Deactivate(bool animated)
        {
            NotifyOnDeactivated();

            StopFade();

            if (!animated)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                _fadeRoutine = StartCoroutine(FadeOut_co());
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

        private void StopFade()
        {
            if (_fadeRoutine != null)
            {
                StopCoroutine(_fadeRoutine);
                _fadeRoutine = null;
            }
        }

        private IEnumerator FadeIn_co()
        {
            var speed = fadeDuration > 0f ? 1f / fadeDuration : float.PositiveInfinity;

            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.deltaTime * speed;
                yield return null;
            }

            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            _fadeRoutine = null;
        }

        private IEnumerator FadeOut_co()
        {
            var speed = fadeDuration > 0f ? 1f / fadeDuration : float.PositiveInfinity;

            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.deltaTime * speed;
                yield return null;
            }

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            _fadeRoutine = null;
        }
    }
}