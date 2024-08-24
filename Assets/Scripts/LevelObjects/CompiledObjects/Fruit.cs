

using System.Collections;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects
{
    public sealed class Fruit : CollectableObject_ScoreIncreaser
    {
        public Fruit(GameObject associatedGameObj):base(associatedGameObj)
        {
            DestroyingCoroutine = TimersManager.Instance_.StartCoroutine(DestroyingFruitAction());
            LevelReseter.LevelWasResetedEvent += ResetLevelAction;
        }

        protected override int AddedScore_ => LevelManager.Instance_.GlobalConsts_.FruitScore;
        public override ILevelPart.LevelPartType PartType_ => ILevelPart.LevelPartType.Fruit;
        private Coroutine DestroyingCoroutine;

        private IEnumerator DestroyingFruitAction()
        {
            yield return new WaitForSeconds(LevelManager.Instance_.GlobalConsts_.FruitDestroyingTime);
            DestroyFruit();
        }
        private void ResetLevelAction()
        {
            DestroyFruit();
        }
        private void DestroyFruit()
        {
            LevelReseter.LevelWasResetedEvent -= ResetLevelAction;
            if (DestroyingCoroutine != null)
                TimersManager.Instance_.StopCoroutine(DestroyingCoroutine);
            GameObject.Destroy(AssociatedGameObj_);
            LevelManager.Instance_.RemoveLevelPart((int)AssociatedGameObj_.transform.position.x,
                (int)AssociatedGameObj_.transform.position.y);
        }
    }
}