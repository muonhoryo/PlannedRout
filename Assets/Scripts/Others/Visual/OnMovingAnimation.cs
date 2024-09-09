using System.Collections;
using System.Collections.Generic;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class OnMovingAnimation : MonoBehaviour
    {
        [SerializeField] private Animator Animator;
        [SerializeField] private MovingComponent Owner;
        [SerializeField] private string MovingAnimationName;
        [SerializeField] private float AnimSpeedMod;

        private void Awake()
        {
            Owner.StartMovingEvent += StartMovingAction;
            Owner.StopMovingEvent += StopMovingAction;
        }
        private void Start()
        {
            Animator.speed = Owner.SpeedProvider_.Speed_.CurrentValue * AnimSpeedMod;
        }
        private void StartMovingAction()
        {
            Animator.SetBool(MovingAnimationName, true);
        }
        private void StopMovingAction()
        {
            Animator.SetBool(MovingAnimationName, false);
        }
    }
}
