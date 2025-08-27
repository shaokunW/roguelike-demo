using System.Collections;
using UnityEngine;

namespace CatAndHuman.UI.select
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class PageBase: MonoBehaviour, IPage
    {
        [Header("Page Animation")]
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField, Range(0.05f, 0.6f)] private float  fadeDuration = 0.05f;

        protected bool busy;
        
        public bool IsBusy => busy;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual IEnumerator Enter(object ctx = null)
        {
            busy = true;
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            float t = 0f, start = canvasGroup.alpha;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, 1f, t / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable   = true;
            busy = false;
        }

        public virtual IEnumerator Exit()
        {
            busy = true;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            float t = 0f, start = canvasGroup.alpha;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, 0f, t / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
            busy = false;
        }
    }
}