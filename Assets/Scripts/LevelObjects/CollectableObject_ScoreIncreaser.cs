
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
        protected abstract int AddedScore_ { get; }
        public abstract ILevelPart.LevelPartType PartType_ { get; }

        public void PickUp()
        {
            ScoreManager.Instance_.AddScore(AddedScore_);
        }
        public void RemovePart()
        {
            GameObject.Destroy(AssociatedGameObj_);
        }
        protected virtual void PickUpAdditionAction() { }
    }
}