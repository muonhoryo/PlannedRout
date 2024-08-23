

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class EnemyBehaviour : MonoBehaviour
    {
        public interface IEnemyBehaviourState
        {
            public Vector2Int Target_ { get;  }

            public void OnStateEnter();
            public void OnStateExit();
        }
        public enum BehaviourStateType:byte
        {
            Dispersion,
            Persecution,
            Scaring,
            BackHome,
            Idle
        }

        public event Action<BehaviourStateType> ChangeBehaviourStateEvent = delegate { };
        public event Action<Vector2Int> TargetIsAchievedEvent = delegate { };

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

        [SerializeField] private MovingComponent MovingScript;

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

            if (MovingScript == null)
                throw new System.Exception("Missing MovingComponent.");

            MovingScript.ChangePositionEvent += ChangePositionAction;
        }
        public IEnemyBehaviourState CurrentState_ { get;private set; }
        public Vector2Int Target_ { get; private set; }

        private List<Vector2Int> PathToTarget;
        private int CurrentPathTarget = 1;

        private void SetBehaviourState(IEnemyBehaviourState state)
        {
            CurrentState_?.OnStateExit();
            CurrentState_ = state;
            CurrentState_.OnStateEnter();
            StartMovingToTarget();
        }

        private void ChangePositionAction(Vector2Int position)
        {
            if (position == Target_)
            {
                TargetIsAchievedEvent(position);
                if (position == Target_)
                    StartMovingToTarget();
            }
            else if (position != PathToTarget[CurrentPathTarget-1])
            {
                StartMovingToTarget();
            }
            else
            {
                MovingScript.ChangeDirection(GetDirectionBetweenPoints(MovingScript.CurrentPosition_, PathToTarget[CurrentPathTarget]));
                CurrentPathTarget++;
            }
        }

        private MovingComponent.MovingDirection GetDirectionBetweenPoints(Vector2Int start,Vector2Int end)
        {
            if (end.x > start.x)
                return MovingComponent.MovingDirection.Right;
            else if (end.x < start.x)
                return MovingComponent.MovingDirection.Left;
            else if (end.y > start.y)
                return MovingComponent.MovingDirection.Top;
            else
                return MovingComponent.MovingDirection.Bottom;
        }


        public void StartMovingToTarget()
        {
            Target_ = CurrentState_.Target_;
            PathFinding pathFinding = new PathFinding(MovingScript.CurrentPosition_, Target_);
            pathFinding.FindPath();
            PathToTarget = pathFinding.Path;
            CurrentPathTarget = 1;

            MovingScript.ChangeDirection(GetDirectionBetweenPoints(MovingScript.CurrentPosition_, PathToTarget[CurrentPathTarget]));
            CurrentPathTarget++;
        }
        public void SelectBehaviourState(BehaviourStateType stateType)
        {
            switch (stateType)
            {
                case BehaviourStateType.Dispersion:
                    SetBehaviourState(DispersionBehaviourState);
                    break;
                case BehaviourStateType.Persecution:
                    SetBehaviourState(PersecutionBehaviourState);
                    break;
                case BehaviourStateType.Scaring:
                    SetBehaviourState(ScaringBehaviourState);
                    break;
                case BehaviourStateType.BackHome:
                    SetBehaviourState(BackHomeBehaviourState);
                    break;
                case BehaviourStateType.Idle:
                    SetBehaviourState(IdleBehaviourState);
                    break;
            }
            ChangeBehaviourStateEvent(stateType);
        }

    }
}