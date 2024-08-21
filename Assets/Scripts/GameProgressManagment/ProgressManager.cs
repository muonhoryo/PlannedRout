

using System;
using UnityEngine;

namespace PlannedRout.GameScoreManagment 
{
    public sealed class ProgressManager:MonoBehaviour
    {
        public static event Action<int> ScoreCountChangedEvent = delegate { };
        public static event Action<int> PointCollectedEvent = delegate { };

        public static ProgressManager Instance_ { get; private set; }

        public int GameScore_ { get; private set; } = 0;
        public int CollectedPointsCount_ { get; private set; } = 0;

        private void Awake()
        {
            if (Instance_ != null)
                throw new System.Exception("Already have ScoreManager.");

            Instance_ = this;
        }

        public void AddScore(int score)
        {
            if (score <= 0)
                throw new System.Exception("Invalid score count.");

            GameScore_ += score;
            ScoreCountChangedEvent(GameScore_);
        }
        public void AddPoint()
        {
            CollectedPointsCount_++;
            PointCollectedEvent(CollectedPointsCount_);
        }
    }
}