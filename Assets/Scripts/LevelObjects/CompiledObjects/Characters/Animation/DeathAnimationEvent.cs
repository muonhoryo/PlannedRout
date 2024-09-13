


using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class DeathAnimationEvent : MonoBehaviour
    {
        [SerializeField] private string MainMenuSceneName;

        public bool IsBackToMenu = false;
        public void ReturnToMainMenu()
        {
            if(IsBackToMenu)
                SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
            else
            {
                LevelReseter.Reset();
                GamePause.Instance_.UnpauseGame();
            }
        }
    }
}