

using PlannedRout.GameScoreManagment;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout
{
    public sealed class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private Text CounterText;

        private void Awake()
        {
            ProgressManager.ScoreCountChangedEvent += ScoreCountChanged;
        }
        private void ScoreCountChanged(int newScore)
        {
            CounterText.text= newScore.ToString();
        }
    }
}