using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlannedRout.Audio
{
    public sealed class MainMenuButtonsSounds : MonoBehaviour
    {
        [SerializeField] private MainMenuButton Owner;
        [SerializeField] private AudioSource HighLightedSoundSource;
        [SerializeField] private AudioSource PressedSoundsSource;

        private void Awake()
        {
            Owner.PointEnterEvent += PointEnter;
            Owner.PointUpEvent += PointUp;
        }
        private void PointEnter()
        {
            HighLightedSoundSource.Play();
        }
        private void PointUp()
        {
            PressedSoundsSource.Play();
        }
    }
}
