using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlannedRout.Visual
{
    public sealed class OnPauseAnimatorStoping : MonoBehaviour
    {
        private event Action OnDestroyEvent = delegate { };

        [SerializeField] private Animator Animator;

        private void Awake()
        {
            GamePause.GamePausedEvent += GamePaused;
        }
        private void OnDestroy()
        {
            GamePause.GamePausedEvent -= GamePaused;
            OnDestroyEvent();
        }
        private void GamePaused()
        {
            GamePause.GamePausedEvent-= GamePaused;
            float speed = Animator.speed;
            Animator.speed = 0;
            void GameUnpaused()
            {
                GamePause.GameUnpausedEvent-= GameUnpaused;
                OnDestroyEvent -= OnDestroyAction;
                GamePause.GamePausedEvent += GamePaused;
                Animator.speed = speed;
            }
            void OnDestroyAction()
            {
                GamePause.GameUnpausedEvent -= GameUnpaused;
            }
            GamePause.GameUnpausedEvent += GameUnpaused;
            OnDestroyEvent += OnDestroyAction;
        }
    }
}
