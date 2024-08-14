

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlannedRout.LevelManagment;

namespace PlannedRout.Editor
{
    public sealed class LevelEditor : MonoBehaviour
    {
        [SerializeField] private int LevelHeight;
        [SerializeField] private int LevelWidth;
        [SerializeField] private string PlayerTag;
        [SerializeField] private string FruitTag;
        [SerializeField] private string EnergyTag;
        [SerializeField] private string PointTag;
        [SerializeField] private string WallTag;
        [SerializeField] private string RoomPointTag;
        [SerializeField] private string EnemyTag_Red;
        [SerializeField] private string EnemyTag_Blue;
        [SerializeField] private string EnemyTag_Pink;
        [SerializeField] private string EnemyTag_Orange;
        [SerializeField] private string DoorTag;

        [ContextMenu("SaveLevel")]
        public void SaveLevel()
        {
            if (LevelHeight <= 0)
                throw new System.Exception("Invalid level height.");
            if (LevelWidth <= 0)
                throw new System.Exception("Invalid level width.");

            LevelData levelData;

            {
                Vector2Int PlayerPosition;
                Vector2Int FruitPosition;
                Vector2Int[] EnergyPositions;
                Vector2Int[] PointsPositions;
                Vector2Int[] WallsPositions;
                Vector2Int RoomPointPosition;
                Vector2Int[] EnemiesPositions;
                Vector2Int DoorPosition;

                Func<GameObject, Vector2Int> selectionFunc = (obj) =>
                {
                    Vector3 pos = obj.transform.position;
                    return new Vector2Int((int)pos.x, (int)pos.y);
                };
                Vector2Int[] FindObjectsPositions(ref string tag)
                {
                    return GameObject.FindGameObjectsWithTag(tag).Select(selectionFunc).ToArray();
                }

                Vector2Int FindObjectPosition(ref string tag)
                {
                    Vector3 pos = GameObject.FindGameObjectWithTag(tag).transform.position;
                    return new Vector2Int((int)pos.x, (int)pos.y);
                }

                PlayerPosition = FindObjectPosition(ref PlayerTag);
                FruitPosition=FindObjectPosition(ref FruitTag);
                EnergyPositions= FindObjectsPositions(ref EnergyTag);
                PointsPositions= FindObjectsPositions(ref PointTag);
                WallsPositions = FindObjectsPositions(ref WallTag);
                RoomPointPosition = FindObjectPosition(ref  RoomPointTag);
                EnemiesPositions = new Vector2Int[]
                {
                    FindObjectPosition(ref EnemyTag_Red),
                    FindObjectPosition(ref EnemyTag_Pink),
                    FindObjectPosition(ref EnemyTag_Blue),
                    FindObjectPosition(ref EnemyTag_Orange)
                };
                DoorPosition = FindObjectPosition(ref DoorTag);

                LevelData.LevelPartType[][] levelMap=new LevelData.LevelPartType[LevelWidth][];
                for(int i=0;i<LevelWidth; i++)
                    levelMap[i]=new LevelData.LevelPartType[LevelHeight];

                void PutPositionInMap(Vector2Int[] array,LevelData.LevelPartType type)
                {
                    foreach (var pos in array)
                        levelMap[pos.x][pos.y] = type;
                }

                levelMap[FruitPosition.x][FruitPosition.y] = LevelData.LevelPartType.Fruit;
                levelMap[DoorPosition.x][FruitPosition.y] = LevelData.LevelPartType.Door;

                PutPositionInMap(EnergyPositions, LevelData.LevelPartType.Energy);
                PutPositionInMap(PointsPositions, LevelData.LevelPartType.Point);
                PutPositionInMap(WallsPositions, LevelData.LevelPartType.Wall);

                levelData=new
                    (levelHeight:LevelHeight,
                    levelWidth:LevelWidth,
                    levelMap: levelMap,
                    playerSpawnPoint: PlayerPosition,
                    enemySpawnPoints: EnemiesPositions,
                    roomPoint: RoomPointPosition,
                    fruitSpawnTriggers: LevelManager.Instance_.LevelData_.FruitSpawnTriggers,
                    globalConsts: LevelManager.Instance_.LevelData_.GlobalConsts);
            } //Level data generation

            LevelSerialization.GenerateLevelSave(levelData, LevelManager.LevelEditorSerializationPath);
        }
    }
}