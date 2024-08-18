

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

        private void Update()
        {
            
        }
        private void ChangeDirection(MovingDirection direction)
        {
            if (MovingDirection_ != direction)
            {
                if (IsHorizontalDirection(direction) == IsHorizontalDirection(MovingDirection_))
                    MovingDirection_ = direction;
                else

            }
        }

        private void Awake()
        {
            SpeedProvider_ = SpeedComponent as ISpeedProvider;
            if (SpeedProvider_ == null)
                throw new System.Exception("Missing speed provider.");
        }
    }
}