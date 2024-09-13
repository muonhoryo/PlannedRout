

using System;
using System.Collections.Generic;
using System.Text;
using PlannedRout.LevelManagment;
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
            Idle
        }

        public event Action<BehaviourStateType> ChangeBehaviourStateEvent = delegate { };
        public event Action<Vector2Int> TargetIsAchievedEvent = delegate { };

        [SerializeField] private MonoBehaviour DispersionBehaviourStateComponent;
        [SerializeField] private MonoBehaviour IdleBehaviourStateComponent;

        private IEnemyBehaviourState DispersionBehaviourState;
        private IEnemyBehaviourState IdleBehaviourState;

        [SerializeField] private MovingComponent MovingScript;

        private void Awake()
        {
            DispersionBehaviourState = DispersionBehaviourStateComponent as IEnemyBehaviourState;
            if (DispersionBehaviourState == null)
                throw new System.Exception("Incorrect behaviour state: Dispersion");
            IdleBehaviourState = IdleBehaviourStateComponent as IEnemyBehaviourState;
            if (IdleBehaviourState == null)
                throw new System.Exception("Incorrect behaviour state: Idle");

            if (MovingScript == null)
                throw new System.Exception("Missing MovingComponent.");

            MovingScript.ChangePositionEvent += ChangePositionAction;
            LevelReseter.LevelWasResetedPostEvent += LevelReseted;
        }
        private void OnDestroy()
        {
            LevelReseter.LevelWasResetedPostEvent -= LevelReseted;
        }
        public IEnemyBehaviourState CurrentState_ { get;private set; }
        public Vector2Int Target_ { get; private set; }

        private List<Vector2Int> PathToTarget;
        private int CurrentPathTarget = 1;

        private Dictionary<(Vector2Int, Vector2Int), List<Vector2Int>> CashedPathes = new Dictionary<(Vector2Int, Vector2Int), List<Vector2Int>>();

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
            void SelectTargetFromState(Vector2Int target)
            {
                Target_ = target;
                (Vector2Int, Vector2Int) pathKey = (MovingScript.CurrentPosition_, Target_);
                if (CashedPathes.ContainsKey(pathKey))
                {
                    PathToTarget = CashedPathes[pathKey];
                    CurrentPathTarget = 1;
                }
                else
                {
                    PathFinding pathFinding = new PathFinding(MovingScript.CurrentPosition_, Target_);
                    pathFinding.FindPath();
                    PathToTarget = pathFinding.Path;
                    CurrentPathTarget = 1;
#if UNITY_EDITOR
                    if (PathToTarget == null||PathToTarget.Count<=1)
                    {
                        throw new Exception("Cannot find path to target: "+MovingScript.CurrentPosition_+"/"+Target_);
                    }
#endif
                    CashedPathes.Add(pathKey, pathFinding.Path);
                }
            }
            SelectTargetFromState(CurrentState_.Target_);

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
                case BehaviourStateType.Idle:
                    SetBehaviourState(IdleBehaviourState);
                    break;
            }
            ChangeBehaviourStateEvent(stateType);
        }

        private void LevelReseted()
        {
            StartMovingToTarget();
        }
    }
}