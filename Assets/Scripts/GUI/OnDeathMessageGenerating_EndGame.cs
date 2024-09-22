

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
        [SerializeField] private string AltMessage_One;
        [SerializeField] private string AltMessage_Two;

        [SerializeField] private string SwapPartIdentifier_CollectedRecord;
        [SerializeField] private string SwapPartIdentifier_Max;
        [SerializeField] private string SwapPartIdentifier_Name;
        [SerializeField] private string SwapPartIdentifier_Number;
        public override void Generate()
        {
            string mesge;
            int pointCount = ProgressManager.Instance_.CollectedPointsCount_;
            if (pointCount % 10 == 1)
            {
                if (pointCount == 11)
                    mesge = Message;
                else
                    mesge = AltMessage_One;
            }
            else if (pointCount % 10 == 2)
            {
                mesge = AltMessage_Two;
            }
            else
            {
                mesge = Message;
            }
            StringBuilder str = new StringBuilder(mesge);
            str.Replace("\\n", "\n");
            str.Replace(SwapPartIdentifier_CollectedRecord, ProgressManager.Instance_.RecordCollectedPoints_.ToString());
            str.Replace(SwapPartIdentifier_Max, LevelManager.Instance_.OnLevelPointCount_.ToString());
            str.Replace(SwapPartIdentifier_Name, PlayerDataContainer.Instance_.CurrentPlayerData_.Name);
            str.Replace(SwapPartIdentifier_Number, PlayerDataContainer.Instance_.CurrentPlayerData_.DataNumber.ToString());
            Owner.text=str.ToString();
        }
    }
}