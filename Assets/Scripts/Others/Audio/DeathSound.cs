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
            PlayerDeath.LifeCountDecreasedEvent+= Death;
        }
        private void OnDestroy()
        {
            PlayerDeath.LifeCountDecreasedEvent-= Death;
        }
        private void Death(int i)
        {
            Source.Play();
        }
    }
}
