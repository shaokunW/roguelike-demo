namespace CatAndHuman
{
    using System;
    using UnityEngine;

    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [DefaultExecutionOrder(-500)]
    public class SafeAreaFitter : MonoBehaviour
    {
        public bool conformX = true, conformY = true;
        public Vector2 extraPadding; // 额外内边距(像素)
        public bool simulateInEditor = false;
        [Range(0f, 0.2f)] public float notchPercent = 0.06f;

        public event Action<Rect> OnSafeAreaChanged;
        public Rect CurrentSafeArea { get; private set; }

        RectTransform _rt;
        Rect _lastSafe;
        Vector2Int _lastScreen;

        void OnEnable()
        {
            _rt = GetComponent<RectTransform>();
            _lastSafe = new Rect(-1, -1, -1, -1);
            Apply();
        }

#if UNITY_EDITOR
        void Update()
        {
            if (!Application.isPlaying && simulateInEditor)
                Apply();
        }
#endif

        void OnRectTransformDimensionsChange()
        {
            if (Application.isPlaying)
                Apply();
        }

        void Apply()
        {
            var sa = GetSafeArea();
            if (sa == _lastSafe && _lastScreen.x == Screen.width && _lastScreen.y == Screen.height)
                return;

            _lastSafe = sa;
            _lastScreen = new Vector2Int(Screen.width, Screen.height);

            Vector2 min = sa.position;
            Vector2 max = sa.position + sa.size;

            if (conformX)
            {
                min.x += extraPadding.x;
                max.x -= extraPadding.x;
            }

            if (conformY)
            {
                min.y += extraPadding.y;
                max.y -= extraPadding.y;
            }

            min.x /= Screen.width;
            min.y /= Screen.height;
            max.x /= Screen.width;
            max.y /= Screen.height;

            if (!conformX)
            {
                min.x = 0;
                max.x = 1;
            }

            if (!conformY)
            {
                min.y = 0;
                max.y = 1;
            }

            _rt.anchorMin = min;
            _rt.anchorMax = max;
            _rt.offsetMin = Vector2.zero;
            _rt.offsetMax = Vector2.zero;

            CurrentSafeArea = sa;
            OnSafeAreaChanged?.Invoke(sa);
        }

        Rect GetSafeArea()
        {
            if (Application.isEditor && simulateInEditor)
            {
                float w = Screen.width, h = Screen.height;
                float top = h * notchPercent;
                float bottom = h * (notchPercent * 0.6f);
                return new Rect(0f, bottom, w, h - top - bottom);
            }

            return Screen.safeArea;
        }
    }
}