using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlannedRout.Audio
{
    public sealed class DeathSound : MonoBehaviour
    {
        [SerializeField] private AudioSource Source;

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
            Source.Play();
        }
    }
}
