
using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects
{
    public abstract class CollectableObject_ScoreIncreaser:ICollectableObject
    {
        public CollectableObject_ScoreIncreaser(GameObject associatedGameObj)
        {
            AssociatedGameObj_=associatedGameObj;
        }

        public GameObject AssociatedGameObj_ { get; private set; }
        public abstract ILevelPart.LevelPartType PartType_ { get; }

        public void PickUp()
        {
            Vector2Int itemPos = AssociatedGameObj_.transform.position.GetIntegerPosition();
            LevelManager.Instance_.RemoveLevelPart(itemPos.x, itemPos.y);
            PickUpAdditionAction();
        }
        public void RemovePart()
        {
            GameObject.Destroy(AssociatedGameObj_);
        }
        protected virtual void PickUpAdditionAction() { }
    }
}