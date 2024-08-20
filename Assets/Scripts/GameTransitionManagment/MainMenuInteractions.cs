

using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout
{
    public sealed class MainMenuInteractions : MonoBehaviour
    {
        [SerializeField] private string GameSceneName;

        public void GameStart()
        {
            SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        }
    }
}