

using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects
{
    public sealed class Point : CollectableObject_ScoreIncreaser
    {
        public Point(GameObject associatedGameObj):base(associatedGameObj) { }

        protected override int AddedScore_ => LevelManager.Instance_.GlobalConsts_.PointScore;
        public override ILevelPart.LevelPartType PartType_ => ILevelPart.LevelPartType.Point;
    }
}