

using System;
using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects.Characters;
using Unity.VisualScripting;
using UnityEngine;

namespace PlannedRout 
{
    public sealed class GamePause : MonoBehaviour
    {
        public static event Action GamePausedEvent = delegate { };
        public static event Action GameUnpausedEvent = delegate { };

        public static GamePause Instance_ { get; private set; }

        public bool IsPaused_ { get; private set; } = false;


        private void Awake()
        {
            Instance_ = this;
        }
        public void PauseGame()
        {
            if (!IsPaused_)
            {
                IsPaused_ = true;
                GamePausedEvent();
            }
        }
        public void UnpauseGame()
        {
            if (IsPaused_)
            {
                IsPaused_ = false;
                GameUnpausedEvent();
            }
        }
    }
}