


using PlannedRout.LevelObjects.Characters;
using UnityEngine;

namespace PlannedRout.Visual
{
    public sealed class LoseAnimationTransition : MonoBehaviour
    {
        [SerializeField] private string TransitionTriggerName;
        [SerializeField] private Animator Animator;
        [SerializeField] private DeathAnimationEvent DeathAnimEvent;

        private void Awake()
        {
            PlayerDeath.LifeCountDecreasedEvent += LifeDecreased;
        }
        private void OnDestroy()
        {
            PlayerDeath.LifeCountDecreasedEvent -= LifeDecreased;
        }
        private void LifeDecreased(int remainedLifes)
        {
            if (remainedLifes <=0)
            {
                DeathAnimEvent.IsBackToMenu = true;
            }
            GamePause.Instance_.PauseGame();
            Animator.SetTrigger(TransitionTriggerName);
        }
    }
}