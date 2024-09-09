


using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class MainMenuTransition : MonoBehaviour
    {
        [SerializeField] private string MainMenuSceneName;
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
        }
    }
}