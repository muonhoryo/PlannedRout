

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

        public int RemainedLifesCount_ { get; private set; }

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            RemainedLifesCount_ = LevelManager.Instance_.GlobalConsts_.PlayerLifeCount;
        }

        public void Death()
        {
            RemainedLifesCount_--;
            if (RemainedLifesCount_ <= 0)
                GameOver();
            else
                ResetGame();
        }
        private void GameOver()
        {
            SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
        }
        private void ResetGame()
        {
            LevelReseter.Reset();
            LifeCountDecreasedEvent(RemainedLifesCount_);
        }
    }
}