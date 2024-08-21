

using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects
{
    public sealed class Energy : CollectableObject_ScoreIncreaser
    {
        public Energy(GameObject associatedGameObj) : base(associatedGameObj) { }

        protected override int AddedScore_ => LevelManager.Instance_.GlobalConsts_.EnergyScore;
        public override ILevelPart.LevelPartType PartType_ => ILevelPart.LevelPartType.Energy;

        protected override void PickUpAdditionAction()
        {
            ProgressManager.Instance_.AddPoint();
            Debug.LogError("THERE SHOULD BE SPEED BUFF AND TRANSITION TO SCARING MODE OF ENEMIES.");
        }
    }
}