

using System;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout
{
    public sealed class PlayerDeath : MonoBehaviour
    {
        public static event Action<int> LifeCountDecreasedEvent = delegate { };

        public int RemainedLifesCount_ { get; private set; }

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            RemainedLifesCount_ = LevelManager.Instance_.GlobalConsts_.PlayerLifesCount;
        }

        public void Death()
        {
            RemainedLifesCount_--;
            LifeCountDecreasedEvent(RemainedLifesCount_);
        }
    }
}