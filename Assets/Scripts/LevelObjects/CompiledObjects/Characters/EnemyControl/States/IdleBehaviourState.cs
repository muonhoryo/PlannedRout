

using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class IdleBehaviourState : MonoBehaviour, EnemyBehaviour.IEnemyBehaviourState
    {
        [SerializeField] private MovingComponent MovingScript;

        private Vector2Int EndTarget;
        private Vector2Int StartTarget;

        public Vector2Int Target_=>MovingScript.CurrentPosition_==StartTarget?EndTarget : StartTarget;

        public void OnStateEnter()
        {
            bool CheckTarget()
            {
                if (LevelManager.Instance_.CheckCellPosition(EndTarget.x, EndTarget.y))
                {
                    ILevelPart cell = LevelManager.Instance_.GetCell(EndTarget.x, EndTarget.y);
                    return cell==null|| cell.PartType_ != ILevelPart.LevelPartType.Wall;
                }
                return false;
            }
            void SetEndTargetAsTop()=>
                EndTarget = new Vector2Int(MovingScript.CurrentPosition_.x, MovingScript.CurrentPosition_.y + 1);
            void SetEndTargetAsBottom() =>
                EndTarget = new Vector2Int(MovingScript.CurrentPosition_.x, MovingScript.CurrentPosition_.y - 1);
            void SetEndTargetAsRight() =>
                EndTarget = new Vector2Int(MovingScript.CurrentPosition_.x + 1, MovingScript.CurrentPosition_.y);
            void SetEndTargetAsLeft() =>
                EndTarget = new Vector2Int(MovingScript.CurrentPosition_.x - 1, MovingScript.CurrentPosition_.y);

            StartTarget = MovingScript.CurrentPosition_;
            SetEndTargetAsTop();
            if (!CheckTarget())
            {
                SetEndTargetAsBottom();
                if (!CheckTarget())
                {
                    SetEndTargetAsRight();
                    if (!CheckTarget())
                        SetEndTargetAsLeft();
                }
            }
        }
        void EnemyBehaviour.IEnemyBehaviourState.OnStateExit() { }

    }
}