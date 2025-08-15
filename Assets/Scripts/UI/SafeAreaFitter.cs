namespace CatAndHuman
{
    using System;
    using UnityEngine;

    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
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
            // 1) 屏幕尺寸未初始化时直接跳过，避免除以 0
            if (Screen.width <= 0 || Screen.height <= 0)
                return;
            var sa = GetSafeArea();
            // 2) （可选）当 SafeArea 本身是空的也跳过
            if (sa.width <= 0 || sa.height <= 0)
                return;
            if (sa == _lastSafe && _lastScreen.x == Screen.width && _lastScreen.y == Screen.height)
                return;

            _lastSafe = sa;
            _lastScreen = new Vector2Int(Screen.width, Screen.height);

            Vector2 min = sa.position;
            Vector2 max = sa.position + sa.size;
            Debug.Log($"[SafeAreaFitter] min={min}, max={max}, screen={_lastScreen}");
            min.x /= Screen.width;
            min.y /= Screen.height;
            max.x /= Screen.width;
            max.y /= Screen.height;

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