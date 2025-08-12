namespace CatAndHuman
{
    using UnityEngine;

    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [DefaultExecutionOrder(-400)]
    public class TopBotSplitLayout : MonoBehaviour
    {
        public RectTransform top;
        public RectTransform bot;

        [Tooltip("负数=top为正方形；正数=top占safeArea高度的比例(0..1)。")]
        public float heightRatio = -1f;

        RectTransform _rt;
        SafeAreaFitter _root;

        void OnEnable()
        {
            _rt = GetComponent<RectTransform>();
            _root = GetComponent<SafeAreaFitter>() ?? GetComponentInParent<SafeAreaFitter>();
            if (_root) _root.OnSafeAreaChanged += OnSafeAreaChanged;
            Rebuild();
        }

        void OnDisable()
        {
            if (_root) _root.OnSafeAreaChanged -= OnSafeAreaChanged;
        }

        void OnRectTransformDimensionsChange()
        {
            // 尺寸变化(旋转/缩放/编辑器改大小)时重排
            Rebuild();
        }

        void OnSafeAreaChanged(Rect r) => Rebuild();

        public void Rebuild()
        {
            if (_rt == null || top == null || bot == null) return;

            Vector2 safeSize = _rt.rect.size;

            float topHeight;
            if (heightRatio > 0f) topHeight = Mathf.Clamp01(heightRatio) * safeSize.y;
            else topHeight = Mathf.Min(safeSize.x, safeSize.y); // 正方形

            float botHeight = Mathf.Max(0, safeSize.y - topHeight);

            // top：顶部居中，固定高(或正方形)，宽=topHeight或等于safe宽(按需求调)
            top.anchorMin = top.anchorMax = new Vector2(0.5f, 1f);
            top.pivot = new Vector2(0.5f, 1f);
            top.sizeDelta = new Vector2(Mathf.Min(safeSize.x, topHeight), topHeight);
            top.anchoredPosition = Vector2.zero;

            // bot：顶部拉伸横向，宽度随容器，高度=剩余
            bot.anchorMin = new Vector2(0f, 1f);
            bot.anchorMax = new Vector2(1f, 1f);
            bot.pivot = new Vector2(0.5f, 1f);
            bot.sizeDelta = new Vector2(0f, botHeight);
            bot.anchoredPosition = new Vector2(0f, -topHeight);
        }
    }
}