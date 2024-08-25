

using PlannedRout.LevelManagment;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class EnemyStatesSpeedModification_Dispersion : EnemyStatesSpeedModification 
    {
        protected override EnemyBehaviour.BehaviourStateType StateType_ => EnemyBehaviour.BehaviourStateType.Dispersion;
        protected override float SpeedModifier_ => LevelManager.Instance_.GlobalConsts_.EnemyDispersionSpeedMod;
    }
}