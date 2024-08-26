

using System;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout
{
    public sealed class PlayerDeath : MonoBehaviour
    {
        public static event Action<int> LifeCountDecreasedEvent = delegate { };

        [SerializeField] private string MainMenuSceneName;

        public void Death()
        {
            GamePause.Instance_.PauseGame();
            SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
        }
    }
}