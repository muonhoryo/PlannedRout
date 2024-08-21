

using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class EnemyBehaviour : MonoBehaviour
    {
        public interface IEnemyBehaviourState
        {
            public Vector2Int Target_ { get; set; }

            public void OnStateEnter();
            public void OnStateExit();
        }

        [SerializeField] private MonoBehaviour DispersionBehaviourStateComponent;
        [SerializeField] private MonoBehaviour PersecutionBehaviourStateComponent;
        [SerializeField] private MonoBehaviour ScaringBehaviourStateComponent;
        [SerializeField] private MonoBehaviour BackHomeBehaviourStateComponent;
        [SerializeField] private MonoBehaviour IdleBehaviourStateComponent;

        private IEnemyBehaviourState DispersionBehaviourState;
        private IEnemyBehaviourState PersecutionBehaviourState;
        private IEnemyBehaviourState ScaringBehaviourState;
        private IEnemyBehaviourState BackHomeBehaviourState;
        private IEnemyBehaviourState IdleBehaviourState;

        private void Awake()
        {
            DispersionBehaviourState = DispersionBehaviourStateComponent as IEnemyBehaviourState;
            if (DispersionBehaviourState == null)
                throw new System.Exception("Incorrect behaviour state: Dispersion");
            PersecutionBehaviourState= PersecutionBehaviourStateComponent as IEnemyBehaviourState;
            if (PersecutionBehaviourState == null)
                throw new System.Exception("Incorrect behaviour state: Persecution");
            ScaringBehaviourState= ScaringBehaviourStateComponent as IEnemyBehaviourState;
            if (ScaringBehaviourState == null)
                throw new System.Exception("Incorrect behaviour state: Scaring");
            BackHomeBehaviourState= BackHomeBehaviourStateComponent as IEnemyBehaviourState;
            if (BackHomeBehaviourState == null)
                throw new System.Exception("Incorrect behaviour state: BackHome");
            IdleBehaviourState = IdleBehaviourStateComponent as IEnemyBehaviourState;
            if (IdleBehaviourState == null)
                throw new System.Exception("Incorrect behaviour state: Idle");
        }

        public IEnemyBehaviourState CurrentState_ { get;private set; }
        public Vector2Int Target_ { get; private set; }

        private void SetBehaviourState(IEnemyBehaviourState state)
        {
            CurrentState_.OnStateExit();
            CurrentState_ = state;
            CurrentState_.OnStateEnter();
            Target_ = state.Target_;
        }
    }
}