

using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout
{
    public sealed class GameEnding : MonoBehaviour 
    {
        [SerializeField] private string MainMenuSceneName;

        private void Awake()
        {
            ProgressManager.PointCollectedEvent += PointCollected;
        }
        private void PointCollected(int count)
        {
            if (count >= LevelManager.Instance_.OnLevelPointCount_)
            {
                EndGame();
            }
        }
        private void EndGame()
        {
            SceneManager.LoadScene(MainMenuSceneName);
        }
    }
}