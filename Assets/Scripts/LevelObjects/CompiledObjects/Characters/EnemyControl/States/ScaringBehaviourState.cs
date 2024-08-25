

using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class ScaringBehaviourState : MonoBehaviour, EnemyBehaviour.IEnemyBehaviourState
    {
        [SerializeField] private MovingComponent MovingScript;

        public Vector2Int Target_
        {
            get
            {
                Vector2Int currPos = MovingScript.CurrentPosition_;
                throw new System.Exception("REALIZE THIS STATE");
            }
        }

        void EnemyBehaviour.IEnemyBehaviourState.OnStateEnter() { }
        void EnemyBehaviour.IEnemyBehaviourState.OnStateExit() { }
    }
}