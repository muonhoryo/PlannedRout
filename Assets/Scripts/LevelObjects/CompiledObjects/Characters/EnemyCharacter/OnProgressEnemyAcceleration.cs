

using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class OnProgressEnemyAcceleration : MonoBehaviour
    {
        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            ProgressManager.PointCollectedEvent += PointCollected;
        }
        private void OnDestroy()
        {
            ProgressManager.PointCollectedEvent -= PointCollected;
        }
        private void PointCollected(int newCount)
        {
            if (newCount >= LevelManager.Instance_.GlobalConsts_.EnemySpeedBuffPointValue)
            {
                void AddSpeedBuff(GameObject enemy)
                {
                    enemy.GetComponent<SpeedComponent>().Speed_.AddModifier_Multiply(LevelManager.Instance_.GlobalConsts_.EnemyPointEatingSpeedMod);
                }
                AddSpeedBuff(LevelManager.Instance_.EnemyCharacter_Red_);
                AddSpeedBuff(LevelManager.Instance_.EnemyCharacter_Blue_);
                AddSpeedBuff(LevelManager.Instance_.EnemyCharacter_Pink_);
                AddSpeedBuff(LevelManager.Instance_.EnemyCharacter_Orange_);
                OnDestroy();
            }
        }
    }
}