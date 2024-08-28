

using System;
using UnityEngine;

namespace PlannedRout.GameScoreManagment 
{
    public sealed class ProgressManager:MonoBehaviour
    {
        public static event Action<int> PointCollectedEvent = delegate { };

        public static ProgressManager Instance_ { get; private set; }

        public int CollectedPointsCount_ { get; private set; } = 0;

        private void Awake()
        {
            if (Instance_ != null)
                throw new System.Exception("Already have ScoreManager.");

            Instance_ = this;
        }

        public void AddPoint()
        {
            CollectedPointsCount_++;
            PointCollectedEvent(CollectedPointsCount_);
        }
    }
}