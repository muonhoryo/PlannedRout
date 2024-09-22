

using System;
using System.Collections;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class MovingComponent : MonoBehaviour
    {
        public event Action<Vector2Int> ChangePositionEvent = delegate { };
        public event Action TunnelTransitionEvent = delegate { };
        public event Action<MovingDirection> ChangeDirectionEvent = delegate { };
        public event Action StartMovingEvent = delegate { };
        public event Action StopMovingEvent = delegate { };

        public enum MovingDirection : byte
        {
            Right,
            Top,
            Left,
            Bottom,
            None
        }

        [SerializeField] private MonoBehaviour SpeedComponent;
        [SerializeField] private bool IsCollideDoor = true;
        public ISpeedProvider SpeedProvider_ { get; private set; }

        public Vector2Int CurrentPosition_ { get; private set; }
        public Vector2Int TargetPosition_ { get; private set; }
        public MovingDirection MovingDirection_ { get; private set; } = MovingDirection.None;
        private Vector2 PhysicDirection;

        private float stepSize;
        public bool IsMoving_ { get; private set; } = false;

        private IEnumerator BugChecking()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);

                Vector2Int physDir = PhysicDirection.GetIntegerPosition();
                void BugFoundAction()
                {
                    Debug.Log("fix bug");
                    transform.position = new Vector3(CurrentPosition_.x,CurrentPosition_.y,transform.position.z);
                    ChangePositionEvent(CurrentPosition_);
                    StopMoving();
                    InternalChangeDirection(MovingDirection_, true);
                }
                if (Vector2Int.Distance(CurrentPosition_, TargetPosition_) > 3)
                {
                    BugFoundAction();
                }
                else
                {
                    Vector2Int vecdiff = TargetPosition_ - CurrentPosition_;
                    Vector2 parsedVecdiff = new Vector2(vecdiff.x, vecdiff.y);
                    Vector2 reqDir = parsedVecdiff.normalized;
                    if (Vector2.Dot(reqDir, parsedVecdiff) < 0.75f)
                    {
                        BugFoundAction();
                    }
                    else
                    {
                        Vector2Int currPos = new Vector2Int((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y));
                        if (LevelManager.Instance_.GetCell(currPos.x, currPos.y) != null &&
                            LevelManager.Instance_.GetCell(currPos.x, currPos.y).PartType_ == ILevelPart.LevelPartType.Wall)
                        {
                            BugFoundAction();
                        }
                    }
                }
            }
        }
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
                ChangePositionEvent(CurrentPosition_);
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
            TargetPosition_ = new Vector2Int(CurrentPosition_.x + physDir.x, CurrentPosition_.y + physDir.y); 
        }
        private void ChangeCurrentCell()
        {
            if (TargetPosition_.x < 0)
            { //Teleport character to right side of map
                transform.position = new Vector2(transform.position.x + LevelManager.Instance_.LevelData_.LvlMap.Width + 1,
                    transform.position.y);

                CurrentPosition_ = new Vector2Int(LevelManager.Instance_.LevelData_.LvlMap.Width - 1, TargetPosition_.y);
                TunnelTransitionEvent();
            }
            else if (TargetPosition_.x >= LevelManager.Instance_.LevelData_.LvlMap.Width)
            { //Teleport character to left side of map
                transform.position = new Vector2(transform.position.x - LevelManager.Instance_.LevelData_.LvlMap.Width - 1,
                    transform.position.y);

                CurrentPosition_ = new Vector2Int(0, TargetPosition_.y);
                TunnelTransitionEvent();
            }
            else if (TargetPosition_.y < 0)
            { //Teleport character to top of map
                transform.position = new Vector2(transform.position.x,
                    transform.position.y + LevelManager.Instance_.LevelData_.LvlMap.Height + 1);

                CurrentPosition_ = new Vector2Int(TargetPosition_.x, LevelManager.Instance_.LevelData_.LvlMap.Height - 1);
                TunnelTransitionEvent();
            }
            else if (TargetPosition_.y >= LevelManager.Instance_.LevelData_.LvlMap.Height)
            { //Teleport character to bottom of map
                transform.position = new Vector2(transform.position.x,
                    transform.position.y - LevelManager.Instance_.LevelData_.LvlMap.Height - 1);

                CurrentPosition_ = new Vector2Int(TargetPosition_.x, 0);
                TunnelTransitionEvent();
            }
            else
            {
                CurrentPosition_ = TargetPosition_;
            }
        }
        private bool CheckMovingPossibility(Vector2Int target)
        {
            if (!LevelManager.Instance_.CheckCellPosition(target.x, target.y))
                return true;
            else
            {
                ILevelPart targetCell = LevelManager.Instance_.GetCell(target.x, target.y);
                if (targetCell != null &&
                    (targetCell.PartType_ == ILevelPart.LevelPartType.Wall ||
                    (IsCollideDoor && targetCell.PartType_ == ILevelPart.LevelPartType.Door)))
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
        private void InternalChangeDirection(MovingDirection direction, bool isEqualHorizontality)
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
                ChangeDirectionEvent(direction);
            }
        }
        private void StartMoving()
        {
            Vector2Int physDir= PhysicDirection.GetIntegerPosition();
            if(!CheckMovingPossibility(new Vector2Int(CurrentPosition_.x + physDir.x, CurrentPosition_.y + physDir.y)))
            {
                StopMoving();
                return;
            }

            enabled = true;
            IsMoving_ = true;
            StartMovingEvent();
        }
        private void StopMoving()
        {
            enabled = false;
            IsMoving_ = false;
            transform.position = new Vector2(CurrentPosition_.x, CurrentPosition_.y);
            StopMovingEvent();
        }

        private void Awake()
        {
            SpeedProvider_ = SpeedComponent as ISpeedProvider;
            if (SpeedProvider_ == null)
                throw new System.Exception("Missing speed provider.");

            enabled = false;
            LevelManager.LevelInitializedEvent += ReferredInitialization;
            GamePause.GamePausedEvent += GamePaused;
            GamePause.GameUnpausedEvent += GameUnpaused;
            LevelReseter.LevelWasResetedEvent += LevelReseted;

            StartCoroutine(BugChecking());
        }
        private void OnDestroy()
        {
            GamePause.GamePausedEvent -= GamePaused;
            GamePause.GameUnpausedEvent -= GameUnpaused;
            LevelReseter.LevelWasResetedEvent -= LevelReseted;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            CurrentPosition_ = transform.position.GetIntegerPosition();
            InternalChangeDirection(MovingDirection.Bottom, true);
        }

        private void GamePaused()
        {
            if (IsMoving_)
                enabled = false;
        }
        private void GameUnpaused()
        {
            if (IsMoving_)
            {
                StopMoving();
                CurrentPosition_ = transform.position.GetIntegerPosition();
                InternalChangeDirection(MovingDirection_, true);
            }
        }
        private void LevelReseted()
        {
            CurrentPosition_ =new Vector2Int((int)transform.position.x,(int)transform.position.y);
            StopMoving();
            InternalChangeDirection(MovingDirection_, true);
        }
    }
}