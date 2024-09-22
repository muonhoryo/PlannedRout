

using System;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.GameScoreManagment 
{
    public sealed class ProgressManager:MonoBehaviour
    {
        public static event Action<int> PointCollectedEvent = delegate { };

        public static ProgressManager Instance_ { get; private set; }

        public int CollectedPointsCount_ { get; private set; } = 0;
        public int RecordCollectedPoints_ { get; private set; } = 0;

        private void Awake()
        {
            if (Instance_ != null)
                throw new System.Exception("Already have ScoreManager.");

            Instance_ = this;
            LevelReseter.LevelWasResetedEvent += LevelReseted;
        }
        private void OnDestroy()
        {
            LevelReseter.LevelWasResetedEvent -= LevelReseted;
        }

        public void AddPoint()
        {
            CollectedPointsCount_++;
            if (CollectedPointsCount_ > RecordCollectedPoints_)
                RecordCollectedPoints_ = CollectedPointsCount_;
            PointCollectedEvent(CollectedPointsCount_);
        }
        private void LevelReseted()
        {
            CollectedPointsCount_ = 0;
            PointCollectedEvent(CollectedPointsCount_);
        }
    }
}