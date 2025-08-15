using UnityEngine;

namespace CatAndHuman
{
    public static class MockSprites
    {
        public static Sprite Solid(int w, int h, Color color)
        {
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            var px = new Color32[w * h];
            var cc = (Color32)color;
            for (var x = 0; x < w; x++) px[x] = cc;
            tex.SetPixels32(px);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0,0,w,h), new Vector2(.5f,.5f), 100f);
        }
    }
}