

using System;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class MovingComponent : MonoBehaviour
    {
        public event Action<Vector2Int> ChangePositionEvent = delegate { };
        public event Action TunnelTransitionEvent = delegate { };
        public event Action<MovingDirection> ChangeDirectionEvent = delegate { };

        public enum MovingDirection:byte
        {
            Right,
            Top,
            Left,
            Bottom,
            None
        }

        [SerializeField] private MonoBehaviour SpeedComponent;
        [SerializeField] private bool IsCollideDoor=true;
        public ISpeedProvider SpeedProvider_ { get; private set; }

        public Vector2Int CurrentPosition_ { get; private set; }
        public Vector2Int TargetPosition_ { get; private set; }
        public MovingDirection MovingDirection_ { get; private set; } = MovingDirection.None;
        private Vector2 PhysicDirection;

        private float stepSize;

        private void Update()
        {
            stepSize = SpeedProvider_.Speed_.CurrentValue * Time.deltaTime;
            if (stepSize > 1)
                stepSize = 1;
            if (Vector2.Distance((Vector2)TargetPosition_, transform.position) <= stepSize)
            {
                ChangeCurrentCell();
                SetMovingTarget();
                if (CheckMovingPossibility(TargetPosition_))
                {
                    Move();
                }
                else
                {
                    StopMoving();
                }
            }
            else
            {
                Move();
            }
        }
        private void Move()
        {
            transform.position += stepSize * (Vector3)PhysicDirection;
        }
        private void SetMovingTarget()
        {
            Vector2Int physDir = PhysicDirection.GetIntegerPosition();
            TargetPosition_ =new Vector2Int(CurrentPosition_.x+ physDir.x, CurrentPosition_.y + physDir.y);
        }
        private void ChangeCurrentCell()
        {
            if (TargetPosition_.x < 0)
            { //Teleport character to right side of map
                transform.position = new Vector2(transform.position.x + LevelManager.Instance_.LevelData_.LvlMap.Width+1,
                    transform.position.y);

                CurrentPosition_ = new Vector2Int(LevelManager.Instance_.LevelData_.LvlMap.Width - 1, TargetPosition_.y);
                TunnelTransitionEvent();
            }
            else if (TargetPosition_.x >= LevelManager.Instance_.LevelData_.LvlMap.Width)
            { //Teleport character to left side of map
                transform.position = new Vector2(transform.position.x - LevelManager.Instance_.LevelData_.LvlMap.Width-1,
                    transform.position.y);

                CurrentPosition_ = new Vector2Int(0, TargetPosition_.y);
                TunnelTransitionEvent();
            }
            else if (TargetPosition_.y < 0)
            { //Teleport character to top of map
                transform.position = new Vector2(transform.position.x,
                    transform.position.y + LevelManager.Instance_.LevelData_.LvlMap.Height+1);

                CurrentPosition_ = new Vector2Int(TargetPosition_.x,LevelManager.Instance_.LevelData_.LvlMap.Height- 1);
                TunnelTransitionEvent();
            }
            else if (TargetPosition_.y >= LevelManager.Instance_.LevelData_.LvlMap.Height)
            { //Teleport character to bottom of map
                transform.position = new Vector2(transform.position.x,
                    transform.position.y - LevelManager.Instance_.LevelData_.LvlMap.Height-1);

                CurrentPosition_ = new Vector2Int(TargetPosition_.x, 0);
                TunnelTransitionEvent();
            }
            else
            {
                CurrentPosition_ = TargetPosition_;
            }

            ChangePositionEvent(CurrentPosition_);
        }
        private bool CheckMovingPossibility(Vector2Int target)
        {
            if(!LevelManager.Instance_.CheckCellPosition(target.x,target.y))
                return true;
            else
            {
                ILevelPart targetCell = LevelManager.Instance_.GetCell(target.x, target.y);
                if (targetCell != null &&
                    (targetCell.PartType_ == ILevelPart.LevelPartType.Wall ||
                    (IsCollideDoor &&targetCell.PartType_ == ILevelPart.LevelPartType.Door)))
                {
                    return false;
                }
                else
                    return true;
            }
        }
        public static bool IsHorizontalDirection(MovingDirection direction) =>
            ((byte)direction & 1) == 0;

        public void ChangeDirection(MovingDirection direction)
        {
            if (MovingDirection_ != direction)
            {
                bool CanHardChangeDirection()
                {
                    float dist = Vector2.Distance((Vector2)CurrentPosition_, transform.position);
                    return dist <= LevelManager.Instance_.GlobalConsts_.MaxToTargetDistanceToChangeMovDirection;
                }

                bool isEqualHorizontality = IsHorizontalDirection(direction) == IsHorizontalDirection(MovingDirection_);
                if (isEqualHorizontality || CanHardChangeDirection())
                {
                    InternalChangeDirection(direction, isEqualHorizontality);
                }
            }
        }
        private void InternalChangeDirection(MovingDirection direction,bool isEqualHorizontality)
        {
            void SetDirectionAsCurrent()
            {
                MovingDirection_ = direction;
                switch (direction)
                {
                    case MovingDirection.Right:
                        PhysicDirection = Vector2.right; break;
                    case MovingDirection.Top:
                        PhysicDirection = Vector2.up; break;
                    case MovingDirection.Bottom:
                        PhysicDirection = Vector2.down; break;
                    case MovingDirection.Left:
                        PhysicDirection = Vector2.left; break;
                }
                SetMovingTarget();
            }
            Vector2Int GetDirectionVector(MovingDirection direction)
            {
                switch (direction)
                {
                    case MovingDirection.Right:
                        return Vector2Int.right;
                    case MovingDirection.Top:
                        return Vector2Int.up;
                    case MovingDirection.Left:
                        return Vector2Int.left;
                    case MovingDirection.Bottom:
                        return Vector2Int.down;
                }
                return Vector2Int.down;
            }

            Vector2Int newTarget = CurrentPosition_ + GetDirectionVector(direction);
            if (CheckMovingPossibility(newTarget))
            {
                SetDirectionAsCurrent();
                if (!enabled)
                {
                    StartMoving();
                }
                else if (!isEqualHorizontality)
                {
                    transform.position = new Vector2(CurrentPosition_.x, CurrentPosition_.y);
                }
            }
            ChangeDirectionEvent(direction);
        }
        private void StartMoving()
        {
            enabled = true;
        }
        private void StopMoving()
        {
            enabled = false;
            transform.position =new Vector2(CurrentPosition_.x,CurrentPosition_.y);
        }

        private void Awake()
        {
            SpeedProvider_ = SpeedComponent as ISpeedProvider;
            if (SpeedProvider_ == null)
                throw new System.Exception("Missing speed provider.");

            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            CurrentPosition_ = transform.position.GetIntegerPosition();
            InternalChangeDirection(MovingDirection.Bottom, true);
        }
    }
}