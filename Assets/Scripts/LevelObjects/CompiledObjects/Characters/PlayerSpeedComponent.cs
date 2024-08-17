

using PlannedRout.LevelManagment;

namespace PlannedRout.LevelObjects.Characters 
{
    public sealed class PlayerSpeedComponent : SpeedComponent
    {
        protected override float DefaultSpeed_ => LevelManager.Instance_.LevelData_.GlobalConsts.PlayerSpeed;
    }
}