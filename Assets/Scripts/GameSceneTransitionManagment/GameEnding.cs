

using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout
{
    public sealed class GameEnding : MonoBehaviour 
    {
        [SerializeField] private Animator Animator;
        [SerializeField] private string AnimTriggerName;

        private void Awake()
        {
            ProgressManager.PointCollectedEvent += PointCollected;
        }
        private void OnDestroy()
        {
            ProgressManager.PointCollectedEvent -= PointCollected;
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
            GamePause.Instance_.PauseGame();
            Animator.SetTrigger(AnimTriggerName);
        }
    }
}