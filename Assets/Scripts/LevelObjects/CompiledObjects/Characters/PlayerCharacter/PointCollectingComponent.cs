

using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects;
using PlannedRout.LevelObjects.Characters;
using UnityEngine;

namespace PlannedRout
{
    public sealed class PointCollectingComponent : MonoBehaviour
    {
        [SerializeField] private MovingComponent Owner;

        private void Awake()
        {
            Owner.ChangePositionEvent += PositionChanged;
        }
        private void PositionChanged(Vector2Int newPos)
        {
            if (LevelManager.Instance_.CheckCellPosition(newPos.x, newPos.y))
            {
                ILevelPart cell=LevelManager.Instance_.GetCell(newPos.x, newPos.y);
                if(cell is ICollectableObject obj)
                {
                    obj.PickUp();
                }
            }
        }
    }
}