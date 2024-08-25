

using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;
using MuonhoryoLibrary;
using System.Collections;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class PointEatingSlower : OnEventSpeedModificator
    {
        protected override float SpeedModDuration_ => LevelManager.Instance_.GlobalConsts_.PointEatingDebuffTime;
        protected override float SpeedMod_ => LevelManager.Instance_.GlobalConsts_.PlayerPointEatingSpeedMod;
        protected override void SubscribeEventAction()
        {
            ProgressManager.PointCollectedEvent += PointCollected;
        }
        private void PointCollected(int i) =>
            SpeedModificationEventAction();
    }
}