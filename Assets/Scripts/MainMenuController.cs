using UnityEngine;

namespace CatAndHuman
{
    public class MainMenuController: MonoBehaviour
    {

        public GameObject LoadPage;
        
        
        public void StartGame()
        {
            GameModeManager.Instance.ChangeGameMode(GameMode.SelectCharacter);
        }
        
        public void LoadGame()
        {
            LoadPage.SetActive(true);
        }


    }
}