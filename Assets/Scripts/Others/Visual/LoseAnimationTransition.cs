


using UnityEngine;

namespace PlannedRout.Visual
{
    public sealed class LoseAnimationTransition : MonoBehaviour
    {
        [SerializeField] private string TransitionTriggerName;
        [SerializeField] private Animator Animator;

        private void Awake()
        {
            PlayerDeath.DeathEvent += Death;
        }
        private void OnDestroy()
        {
            PlayerDeath.DeathEvent -= Death;
        }
        private void Death()
        {
            Animator.SetTrigger(TransitionTriggerName);
        }
    }
}