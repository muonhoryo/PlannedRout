


using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects.Characters;
using UnityEngine;

namespace PlannedRout.Visual
{
    public sealed class LoseAnimationTransition : MonoBehaviour
    {
        [SerializeField] private string TransitionTriggerName;
        [SerializeField] private string ResetTriggerName;
        [SerializeField] private Animator Animator;

        private void Awake()
        {
            PlayerDeath.LifeCountDecreasedEvent += LifeDecreased;
            LevelReseter.LevelWasResetedEvent += LevelWasReseted;
        }
        private void OnDestroy()
        {
            PlayerDeath.LifeCountDecreasedEvent -= LifeDecreased;
            LevelReseter.LevelWasResetedEvent -= LevelWasReseted;
        }
        private void LifeDecreased(int remainedLifes)
        {
            GamePause.Instance_.PauseGame();
            Animator.SetTrigger(TransitionTriggerName);
        }
        private void LevelWasReseted()
        {
            Animator.SetTrigger(ResetTriggerName);
        }
    }
}