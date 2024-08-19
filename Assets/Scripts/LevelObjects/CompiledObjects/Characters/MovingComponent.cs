

using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class MovingComponent : MonoBehaviour
    {
        public enum MovingDirection:byte
        {
            Right,
            Top,
            Left,
            Bottom
        }
        public static bool IsHorizontalDirection(MovingDirection direction) =>
            ((byte)direction & 254) == 0;

        [SerializeField] private MonoBehaviour SpeedComponent;
        public ISpeedProvider SpeedProvider_ { get; private set; }

        public Vector2Int CurrentPosition_ { get; private set; }
        public Vector2Int TargetPosition_ { get; private set; }
        public MovingDirection MovingDirection_ { get; private set; } = MovingDirection.Bottom;
        private Vector2 PhysicDirection = Vector2.down;

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
                if (CheckMovingPossibility())
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
            TargetPosition_ =new Vector2Int(CurrentPosition_.x+(int)Mathf.Round(PhysicDirection.x), CurrentPosition_.y + (int)Mathf.Round(PhysicDirection.y));
        }
        private void ChangeCurrentCell()
        {
            if (TargetPosition_.x < 0)
            { //Teleport character to right side of map
                transform.position = new Vector2(transform.position.x + LevelManager.Instance_.LevelData_.LvlMap.Width,
                    transform.position.y);

                CurrentPosition_ = new Vector2Int(LevelManager.Instance_.LevelData_.LvlMap.Width - 1, TargetPosition_.y);
            }
            else if (TargetPosition_.x >= LevelManager.Instance_.LevelData_.LvlMap.Width)
            { //Teleport character to left side of map
                transform.position = new Vector2(transform.position.x - LevelManager.Instance_.LevelData_.LvlMap.Width,
                    transform.position.y);

                CurrentPosition_ = new Vector2Int(0, TargetPosition_.y);
            }
            else if (TargetPosition_.y < 0)
            { //Teleport character to top of map
                transform.position = new Vector2(transform.position.x,
                    transform.position.y + LevelManager.Instance_.LevelData_.LvlMap.Height);

                CurrentPosition_ = new Vector2Int(TargetPosition_.x,LevelManager.Instance_.LevelData_.LvlMap.Height- 1);
            }
            else if (TargetPosition_.y >= LevelManager.Instance_.LevelData_.LvlMap.Height)
            { //Teleport character to bottom of map
                transform.position = new Vector2(transform.position.x,
                    transform.position.y - LevelManager.Instance_.LevelData_.LvlMap.Height);

                CurrentPosition_ = new Vector2Int(TargetPosition_.x, 0);
            }
            else
            {
                CurrentPosition_ = TargetPosition_;
            }
        }
        private bool CheckMovingPossibility()
        {
            if(!LevelManager.Instance_.CheckCellPosition(TargetPosition_.x,TargetPosition_.y))
                return true;
            else
            {
                ILevelPart targetCell = LevelManager.Instance_.GetCell(TargetPosition_.x, TargetPosition_.y);
                if (targetCell != null &&
                    (targetCell.PartType_ == ILevelPart.LevelPartType.Wall ||
                    targetCell.PartType_ == ILevelPart.LevelPartType.Door))
                {
                    return false;
                }
                else
                    return true;
            }
        }

        private void ChangeDirection(MovingDirection direction)
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
                        PhysicDirection=Vector2.left; break;
                }
            }
            bool CanHardChangeDirection()
            {
                float dist = Vector2.Distance((Vector2)TargetPosition_, transform.position);
                return dist <= LevelManager.Instance_.GlobalConsts_.MaxToTargetDistanceToChangeMovDirection;
            }

            if (MovingDirection_ != direction)
            {
                if (IsHorizontalDirection(direction) == IsHorizontalDirection(MovingDirection_) ||
                    CanHardChangeDirection())
                {
                    SetDirectionAsCurrent();
                    if (!enabled&&
                        CheckMovingPossibility())
                    {
                        StartMoving();
                    }
                }
            }
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
        }
    }
}