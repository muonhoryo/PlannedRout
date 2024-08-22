

using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class DispersionBehaviourState : MonoBehaviour, EnemyBehaviour.IEnemyBehaviourState
    {
        public enum DispersionTarget:byte
        {
            RightTop,
            LeftTop,
            RightBottom,
            LeftBottom
        }

        [SerializeField] private DispersionTarget Target;

        public Vector2Int Target_ { get; private set; }

        private Vector2Int[] Corners;
        private int CurrentCornerIndex = 0;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += DeferredInitialization;
        }
        private void DeferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= DeferredInitialization;

            bool isRight =((byte)Target&1)==0;
            bool isTop =((byte)Target&2)==0;
            int targetX = isRight ? LevelManager.Instance_.LevelData_.LvlMap.Width - 2 : 1;
            int targetY = isTop ? LevelManager.Instance_.LevelData_.LvlMap.Height - 2 : 1;

            Target_ = new Vector2Int(targetX, targetY);
        }

        Vector2Int EnemyBehaviour.IEnemyBehaviourState.Target_ => Target_;
        void EnemyBehaviour.IEnemyBehaviourState.OnStateEnter() { }
        void EnemyBehaviour.IEnemyBehaviourState.OnStateExit() { }
    }
}