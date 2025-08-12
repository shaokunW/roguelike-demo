using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatAndHuman
{
    public enum GameMode
    {
        Loading, MainMenu, SelectCharacter, InGame
    }
    
    
    public class GameModeManager: MonoBehaviour
    {
        public static GameModeManager Instance { get; private set; }
        
        public GameMode gameMode;
        
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

        private void Start()
        {
            if (gameMode == GameMode.Loading)
            {
                ChangeGameMode(GameMode.MainMenu);
            }

        }

        public void ChangeGameMode(GameMode newGameMode)
        {
            gameMode = newGameMode;
            var targetScene = gameMode switch
            {
                GameMode.MainMenu => "Main Menu Portrait",
                GameMode.SelectCharacter => "Select Character Portrait",
                GameMode.InGame => "Level 1 Portrait",
                _ => null
            };
            if (!string.IsNullOrEmpty(targetScene) &&
                SceneManager.GetActiveScene().name != targetScene)
            {
                SceneManager.LoadScene(targetScene);
            }
        }
        
    }
}