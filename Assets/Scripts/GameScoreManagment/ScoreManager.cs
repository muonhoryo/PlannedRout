

using UnityEngine;

namespace PlannedRout.GameScoreManagment 
{
    public sealed class ScoreManager:MonoBehaviour
    {
        public static ScoreManager Instance_ { get; private set; }

        public int GameScore_ { get; private set; }

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
        }
    }
}