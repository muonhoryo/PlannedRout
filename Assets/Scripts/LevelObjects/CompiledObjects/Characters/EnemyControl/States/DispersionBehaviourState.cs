

using System.Linq;
using System.Text;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.U2D;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class DispersionBehaviourState : MonoBehaviour, EnemyBehaviour.IEnemyBehaviourState
    {
        public enum DispersionTarget : byte
        {
            RightTop,
            LeftTop,
            RightBottom,
            LeftBottom
        }

        [SerializeField] private DispersionTarget Target;
        [SerializeField] private MovingComponent MovingScript;

        public Vector2Int Target_
        {
            get
            {
                if (CurrentCornerIndex == Corners.Length)
                    CurrentCornerIndex = 0;
                return Corners[CurrentCornerIndex++];
            }
        }

        private Vector2Int[] Corners;
        private int CurrentCornerIndex = 0;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;

            bool isRight = ((byte)Target & 1) == 0;
            bool isTop = ((byte)Target & 2) == 0;

            int rightBorder = LevelManager.Instance_.LevelData_.LvlMap.Width - 2;
            int topBorder = LevelManager.Instance_.LevelData_.LvlMap.Height - 2;
            int widthCenter = LevelManager.Instance_.LevelData_.LvlMap.Width / 2;
            int heightCenter = LevelManager.Instance_.LevelData_.LvlMap.Height / 2;

            Corners = new Vector2Int[4]
            {
                new Vector2Int(
                    isRight ? rightBorder: 1,
                    isTop ? topBorder: 1),
                new Vector2Int(
                    isRight?rightBorder:1,
                    heightCenter),
                new Vector2Int(
                    widthCenter,
                    heightCenter),
                new Vector2Int(
                    widthCenter,
                    isTop?topBorder:1)
            };
            for (int i = 0; i < 4; i++)
            {
                Vector2Int cellPos = Corners[i];
                void FindOtherPoint()
                {
                    Corners[i] = LevelManager.Instance_.GetNearestPassableCell(cellPos, MovingScript.CurrentPosition_, false);
                }

                if (MovingScript.CurrentPosition_ == Corners[i])
                    FindOtherPoint();
                else
                {
                    ILevelPart cell = LevelManager.Instance_.GetCell(cellPos.x, cellPos.y);
                    if (cell != null && cell.PartType_ == ILevelPart.LevelPartType.Wall)
                        FindOtherPoint();
                    else
                    {
                        PathFinding pathFinding = new PathFinding(MovingScript.CurrentPosition_, Corners[i]);
                        pathFinding.FindPath();
                        if (pathFinding.Path == null)
                            FindOtherPoint();
                        else
                        {
                            StringBuilder str2 = new StringBuilder();
                            foreach (var j in pathFinding.Path)
                                str2.Append(j + "---");
                            Debug.Log(str2);
                        }
                    }
                }
            }
            StringBuilder str = new StringBuilder();
            foreach (var i in Corners)
                str.Append(i + "---");
            Debug.Log(str); 
        }

        Vector2Int EnemyBehaviour.IEnemyBehaviourState.Target_ => Target_;
        public void OnStateEnter()
        {
            CurrentCornerIndex = MovingScript.CurrentPosition_ != Corners[0] ? 0 : 1;
        }
        void EnemyBehaviour.IEnemyBehaviourState.OnStateExit() { }
    }
}