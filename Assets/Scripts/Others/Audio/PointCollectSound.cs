using System.Collections;
using System.Collections.Generic;
using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.Audio
{
    public sealed class PointCollectSound : MonoBehaviour
    {
        [SerializeField] private AudioSource CommonSoundSource;
        [SerializeField] private AudioSource WinSoundSource;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            ProgressManager.PointCollectedEvent += PointCollected;
        }
        private void OnDestroy()
        {
            ProgressManager.PointCollectedEvent -= PointCollected;
        }
        private void PointCollected(int count)
        {
            if (count >= LevelManager.Instance_.OnLevelPointCount_)
            {
                WinSoundSource.Play(); 
            }
            else
            {
                CommonSoundSource.Play();
            }
        }
    }
}
