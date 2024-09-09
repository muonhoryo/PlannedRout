

using System;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout
{
    public sealed class PlayerDeath : MonoBehaviour
    {
        public static event Action DeathEvent = delegate { };

        private bool IsDead = false;

        public void Death()
        {
            if (!IsDead)
            {
                IsDead = true;
                GamePause.Instance_.PauseGame();
                DeathEvent();
            }
        }
    }
}