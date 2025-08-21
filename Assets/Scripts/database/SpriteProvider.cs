using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CatAndHuman
{
    public class SpriteProvider
    {
        private static SpriteProvider _inst;
        public static SpriteProvider Instance => _inst ??= new SpriteProvider();

        class Entry
        {
            public Sprite Sprite;
            public AsyncOperationHandle<Sprite> Handle;
            public int RefCount;
            public Task<Sprite> InFlightTask;
        }

        readonly Dictionary<string, Entry> _map = new();

        public async Task<Sprite> AcquireAsync(string key, Sprite fallback = null)
        {
            if (string.IsNullOrEmpty(key)) return fallback;
            if (_map.TryGetValue(key, out var e))
            {
                if (e.InFlightTask != null)
                {
                    e.RefCount++;
                    return await e.InFlightTask;
                }

                e.RefCount++;
                return e.Sprite;
            }

            var ne = new Entry { RefCount = 1 };
            _map[key] = ne;
            ne.InFlightTask = LoadInternal(key, ne, fallback);
            try
            {
                return await ne.InFlightTask;
            }
            finally
            {
                ne.InFlightTask = null;
            }
        }

        async Task<Sprite> LoadInternal(string key, Entry e, Sprite fallback)
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<Sprite>(key);
                e.Handle = handle;
                var sp = await handle.Task;
                e.Sprite = sp != null ? sp : fallback;
                return e.Sprite;
            }
            catch
            {
                Debug.LogWarning($"[SpriteProvider] Load failed: {key}");
                e.Sprite = fallback;
                return e.Sprite;
            }
        }
        
        
        public void Release(string key)
        {
            if (!_map.TryGetValue(key, out var e)) return;
            if (--e.RefCount > 0) return;

            if (e.Handle.IsValid()) Addressables.Release(e.Handle);
            _map.Remove(key);
        }
        
        public void ClearAll()
        {
            foreach (var kv in _map)
                if (kv.Value.Handle.IsValid()) Addressables.Release(kv.Value.Handle);
            _map.Clear();
        }
    }
}