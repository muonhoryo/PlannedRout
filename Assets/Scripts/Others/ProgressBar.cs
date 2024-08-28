

using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout
{
    public sealed class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image MaterialOwner;
        [SerializeField] private string ThresholdName;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
            MaterialOwner.material = Instantiate(MaterialOwner.material);
            ProgressManager.PointCollectedEvent += PointCollected;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            PointCollected(0);
        }
        private void OnDestroy()
        {
            ProgressManager.PointCollectedEvent -= PointCollected;
        }
        private void PointCollected(int newCount)
        {
            MaterialOwner.material.SetFloat(ThresholdName, (float)newCount / (float)LevelManager.Instance_.OnLevelPointCount_);
        }
    }
}