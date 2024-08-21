

using PlannedRout.GameScoreManagment;
using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects;
using UnityEngine;

namespace PlannedRout
{
    public sealed class FruitSpawner : MonoBehaviour
    {
        private int SpawnedFruitCount = 0;
        private GameObject AssociatedObj=null;

        private void Awake()
        {
            ProgressManager.PointCollectedEvent += PointCountChanged;
        }
        private void PointCountChanged(int pointCount)
        {
            if (pointCount >= LevelManager.Instance_.LevelData_.FruitSpawnTriggers[SpawnedFruitCount])
            {
                if(AssociatedObj==null)
                {
                    AssociatedObj = GameObject.Instantiate(LevelLoadingData.Instance_.LevelObjsPrefabs.FruitPrefab_,
                        LevelLoadingData.Instance_.LevelParentObject.transform);
                    Vector2Int spawnPoint = LevelManager.Instance_.LevelData_.FruitSpawnPoint;
                    AssociatedObj.transform.position = spawnPoint.GetPhysicsPosition();
                    Fruit fruit = new Fruit(AssociatedObj);
                    LevelManager.Instance_.AddLevelPart(fruit, spawnPoint.x, spawnPoint.y);
                }
                SpawnedFruitCount++;
                if (LevelManager.Instance_.LevelData_.FruitSpawnTriggers.Length == SpawnedFruitCount)
                    ProgressManager.PointCollectedEvent -= PointCountChanged;
            }
        }
    }
}