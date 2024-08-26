
using System.Collections;
using MuonhoryoLibrary;
using PlannedRout.GameScoreManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters 
{
    public abstract class OnEventSpeedModificator : MonoBehaviour
    {
        [SerializeField] private SpeedComponent SlowdownTarget;

        private CompositeParameter<float>.IConstModifier<float> CurrentSlowdown = null;

        private void Awake()
        {
            SubscribeEventAction();
        }
        private void OnDestroy()
        {
            UnsubscribeEventAction();
        }
        protected void SpeedModificationEventAction()
        {
            if (CurrentSlowdown == null)
            {
                AddNewSlowdown();
            }
            else
            {
                UpdateCurrentSlowdown();
            }
        }
        private void AddNewSlowdown()
        {
            CurrentSlowdown = SlowdownTarget.Speed_.AddModifier_Multiply(SpeedMod_);
            StartCoroutine(SlowdownTimer());
        }
        private void UpdateCurrentSlowdown()
        {
            StopAllCoroutines();
            StartCoroutine(SlowdownTimer());
        }
        private IEnumerator SlowdownTimer()
        {
            yield return new WaitForSeconds(SpeedModDuration_);
            CurrentSlowdown.RemoveModifier();
            CurrentSlowdown = null;
        }

        protected abstract float SpeedMod_ { get; }
        protected abstract float SpeedModDuration_ { get; }
        protected abstract void SubscribeEventAction();
        protected abstract void UnsubscribeEventAction();
    }
}