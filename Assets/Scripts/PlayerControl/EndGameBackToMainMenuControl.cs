

using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout.PlayerControl
{
    public sealed class EndGameBackToMainMenuControl : MonoBehaviour
    {
        [SerializeField] private string BackToMainMenuInputName;
        [SerializeField] private string MainMenuSceneName;

        private void Awake()
        {
            enabled = false;
            PlayerDeath.LifeCountDecreasedEvent += LifeCountDecreased;
            ProgressManager.PointCollectedEvent += PointCollected;
        }
        private void OnDestroy()
        {
            PlayerDeath.LifeCountDecreasedEvent -= LifeCountDecreased;
            ProgressManager.PointCollectedEvent -= PointCollected;
        }
        private void Update()
        {
            if (Input.GetButtonDown(BackToMainMenuInputName))
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }
        }
        private void LifeCountDecreased(int count)
        {
            if (count <= 0)
            {
                EndGame();
            }
        }
        private void PointCollected(int pointCount)
        {
            if (pointCount >= LevelManager.Instance_.OnLevelPointCount_)
            {
                EndGame();
            }
        }
        private void EndGame()
        {
            PlayerDeath.LifeCountDecreasedEvent -= LifeCountDecreased;
            enabled = true;
        }
    }
}