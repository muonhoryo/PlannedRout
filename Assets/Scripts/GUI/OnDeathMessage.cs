using System.Collections;
using System.Collections.Generic;
using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout.Visual
{
    public sealed class OnDeathMessage : MonoBehaviour
    {
        [SerializeField] private Animator MessageAnimator;
        [SerializeField] private string DeathAnimTriggerName;
        [SerializeField] private string EndGameAnimTriggerName;

        [SerializeField] private OnDeathMessageGenerating DeathMessage;
        [SerializeField] private OnDeathMessageGenerating WinMessage;
        [SerializeField] private OnDeathMessageGenerating LoseMessage;
        [SerializeField] private DeathAnimationEvent DeathAnimation;

        private void Awake()
        {
            PlayerDeath.LifeCountDecreasedEvent += LifeCountDecreased;
            ProgressManager.PointCollectedEvent += PointCollected;
        }
        private void OnDestroy()
        {
            PlayerDeath.LifeCountDecreasedEvent -= LifeCountDecreased;
            ProgressManager.PointCollectedEvent -= PointCollected;
        }
        private void LifeCountDecreased(int count)
        {
            if (count > 0)
            {
                DeathMessage.Generate();
                MessageAnimator.SetTrigger(DeathAnimTriggerName);
            }
            else
            {
                DeathAnimation.IsBackToMenu = true;
                if (ProgressManager.Instance_.CollectedPointsCount_ < LevelManager.Instance_.GlobalConsts_.CollectedPointToWin)
                {
                    LoseMessage.Generate();
                }
                else
                {
                    WinMessage.Generate();
                }
                MessageAnimator.SetTrigger(EndGameAnimTriggerName);
            }
        }
        private void PointCollected(int count)
        {
            if (count >= LevelManager.Instance_.OnLevelPointCount_)
            {
                WinMessage.Generate();
                MessageAnimator.SetTrigger(EndGameAnimTriggerName);
            }
        }
    }
}
