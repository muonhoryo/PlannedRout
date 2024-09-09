using System.Collections;
using System.Collections.Generic;
using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects.Characters;
using UnityEngine;

namespace PlannedRout.Audio
{
    public sealed class MovingSound : MonoBehaviour
    {
        [SerializeField] private AudioSource Source;
        [SerializeField] private MovingComponent Owner;

        private float DelayTimeMod;
        private Coroutine PlayingLoop=null;

        private void Awake()
        {
            Owner.StartMovingEvent += StartMoving;
            LevelManager.LevelInitializedEvent += ReferredInitialization;
            GamePause.GamePausedEvent += GamePaused;
        }
        private void OnDestroy()
        {
            GamePause.GamePausedEvent -= GamePaused;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            DelayTimeMod = LevelManager.Instance_.GlobalConsts_.StepsSoundDelayTimeModifier;
        }
        private void StartMoving()
        {
            Owner.StartMovingEvent -= StartMoving;
            Owner.StopMovingEvent += StopMoving;
            PlayingLoop = StartCoroutine(SoundPlayingLoop());
        }

        private void StopMoving()
        {
            Owner.StopMovingEvent -= StopMoving;
            Owner.StartMovingEvent += StartMoving;
            StopCoroutine(PlayingLoop);
            Source.Stop();
        }

        private IEnumerator SoundPlayingLoop()
        {
            while (true)
            {
                Source.Play();
                yield return new WaitForSeconds(DelayTimeMod / Owner.SpeedProvider_.Speed_.CurrentValue);
            }
        }

        private void GamePaused()
        {
            if (PlayingLoop != null)
            {
                StopCoroutine(PlayingLoop);
            }
        }
    }
}
