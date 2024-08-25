

using PlannedRout.LevelManagment;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class EnemyTunnelTransitionSlower : OnEventSpeedModificator
    {
        protected override float SpeedModDuration_ => LevelManager.Instance_.GlobalConsts_.TunnelTransitionDebuffTime;
        protected override float SpeedMod_ => LevelManager.Instance_.GlobalConsts_.EnemyTunnelSpeedMod;

        protected override void SubscribeEventAction()
        {
            GetComponent<MovingComponent>().TunnelTransitionEvent += SpeedModificationEventAction;
        }
    }
}