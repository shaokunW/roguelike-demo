using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CatAndHuman
{
    public  sealed class SpriteLease : IDisposable
    {
        readonly string _key;
        private bool _disposed;

        private SpriteLease(string key)
        {
            _key = key;
        }
        
        public static async Task<(Sprite, SpriteLease)> GetAsync(string key, Sprite fallback=null)
        {
            var sp = await SpriteProvider.Instance.AcquireAsync(key, fallback);
            return (sp, new SpriteLease(key));
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            SpriteProvider.Instance.Release(_key);
        }
    }
}