

using System;
using System.Text;
using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using PlannedRout.PlayersRegistry;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout.Visual
{
    public sealed class OnDeathMessageGenerating_EndGame : OnDeathMessageGenerating
    {
        [SerializeField] private Text Owner;
        [SerializeField] private string Message;
        [SerializeField] private string SwapPartIdentifier_Collected;
        [SerializeField] private string SwapPartIdentifier_Max;
        [SerializeField] private string SwapPartIdentifier_Name;
        [SerializeField] private string SwapPartIdentifier_Number;
        public override void Generate()
        {
            StringBuilder str = new StringBuilder(Message);
            str.Replace(SwapPartIdentifier_Collected, ProgressManager.Instance_.CollectedPointsCount_.ToString());
            str.Replace(SwapPartIdentifier_Max, LevelManager.Instance_.OnLevelPointCount_.ToString());
            str.Replace(SwapPartIdentifier_Name, PlayerDataContainer.Instance_.CurrentPlayerData_.Name);
            str.Replace(SwapPartIdentifier_Number, PlayerDataContainer.Instance_.CurrentPlayerData_.DataNumber.ToString());
            Owner.text=str.ToString();
        }
    }
}