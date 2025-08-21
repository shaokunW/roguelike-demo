using UnityEngine;

namespace CatAndHuman.Utilities
{
    public class GlobalGameState: MonoBehaviour
    {
        public static GlobalGameState Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Reset()
        {
            
        }

        public void InitializeGameDta()
        {
        }

    }
}