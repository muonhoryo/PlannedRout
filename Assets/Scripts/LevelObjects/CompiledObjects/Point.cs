

using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects
{
    public sealed class Point : CollectableObject_ScoreIncreaser
    {
        public Point(GameObject associatedGameObj):base(associatedGameObj) { }

        public override ILevelPart.LevelPartType PartType_ => ILevelPart.LevelPartType.Point;

        protected override void PickUpAdditionAction()
        {
            ProgressManager.Instance_.AddPoint();
        }
    }
}