
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
            LevelReseter.LevelWasResetedEvent += LevelReseted;
            PlayerDeath.LifeCountDecreasedEvent += LifeDecreaased;
            ProgressManager.PointCollectedEvent += PointCollected;
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
            AssociatedGameObj_.SetActive(false);
        }
        protected virtual void PickUpAdditionAction() { }

        private void LevelReseted()
        {
            if (!AssociatedGameObj_.activeSelf)
            {
                Vector2Int itemPos = AssociatedGameObj_.transform.position.GetIntegerPosition();
                LevelManager.Instance_.AddLevelPart(this, itemPos.x, itemPos.y);
                AssociatedGameObj_.SetActive(true);
            }
        }
        private void LifeDecreaased(int count)
        {
            if (count <= 0)
                ResetSubscribing();
        }
        private void PointCollected(int count)
        {
            if (count >= LevelManager.Instance_.OnLevelPointCount_)
                ResetSubscribing();
        }
        private void ResetSubscribing()
        {
            LevelReseter.LevelWasResetedEvent -= LevelReseted;
            PlayerDeath.LifeCountDecreasedEvent -= LifeDecreaased;
            ProgressManager.PointCollectedEvent -= PointCollected;
        }
    }
}