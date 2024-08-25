

using MuonhoryoLibrary;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters 
{
    public abstract class EnemyStatesSpeedModification : MonoBehaviour
    {
        [SerializeField] private SpeedComponent SpeedModTarget;
        [SerializeField] private EnemyBehaviour Owner;

        private CompositeParameter.IConstModifier<float> CurrentModifier;

        private void Awake()
        {
            Owner.ChangeBehaviourStateEvent += ChangeState_Inactive;
        }
        private void ChangeState_Inactive(EnemyBehaviour.BehaviourStateType newState)
        {
            if (newState == StateType_)
                EnterState();
        }
        private void ChangeState_Active(EnemyBehaviour.BehaviourStateType newState)
        {
            if (newState != StateType_)
                ExitState();
        }
        private void EnterState()
        {
            if (CurrentModifier == null)
            {
                CurrentModifier = SpeedModTarget.Speed_.AddModifier_Multiply(SpeedModifier_);
                Owner.ChangeBehaviourStateEvent-= ChangeState_Inactive;
                Owner.ChangeBehaviourStateEvent += ChangeState_Active;
            }
        }
        private void ExitState()
        {
            CurrentModifier.RemoveModifier();
            CurrentModifier = null;
            Owner.ChangeBehaviourStateEvent+= ChangeState_Inactive;
            Owner.ChangeBehaviourStateEvent -= ChangeState_Active;
        }

        protected abstract float SpeedModifier_ { get; }
        protected abstract EnemyBehaviour.BehaviourStateType StateType_ { get; }
    }
}