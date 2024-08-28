

using PlannedRout.GameScoreManagment;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout
{
    public sealed class PointCounter : MonoBehaviour
    {
        [SerializeField] private Text CounterText;

        private void Awake()
        {
            ProgressManager.PointCollectedEvent += ScoreCountChanged;
        }
        private void OnDestroy()
        {
            ProgressManager.PointCollectedEvent -= ScoreCountChanged;
        }
        private void ScoreCountChanged(int newScore)
        {
            CounterText.text= newScore.ToString();
        }
    }
}