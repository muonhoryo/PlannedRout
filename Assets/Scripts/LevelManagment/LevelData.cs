

using System;
using UnityEngine;

namespace PlannedRout.LevelManagment
{
    [Serializable]
    public struct LevelData
    {
        [Serializable]
        public struct GlobalConstData
        {
            //Times
            public float FruitDestroyingTime;
            public float EnergyBuffTime;
            public float EnemyDispersionTime;
            public float PlayerAFKTime;
            public float NextDispersionTime;
            //Speed buffs
            public float EnergyPlayerSpeedMod;
            public float EnemyPointEatingSpeedMod;
            public float EnemyHomeBackSpeedMod;
            public float RedSpecialSpeedMod;
            //Speed debuffs
            public float PlayerPointEatingSpeedMod;
            public float EnemyDispersionSpeedMod;
            public float EnemyTunnelSpeedMod;
            public float EnemyScaringSpeedMod;
            //Speed
            public float PlayerSpeed;
            public float EnemySpeed;
            //Point values
            public int EnemySpeedBuffPointValue;
            public int RedSpeedBuffPointValue;
            public int NextRoomExitPointValue;
            //Others
            public int MaxDispersionCount;
            public int PlayerLifeCount;
            public float EnemyScoreModifier;
            //Given score
            public int FruitScore;
            public int PointScore;
            public int EnergyScore;
            public int EnemyScore;

            public GlobalConstData(float fruitDestroyingTime,
                                   float energyBuffTime,
                                   float enemyDispersionTime,
                                   float playerAFKTime,
                                   float nextDispersionTime,
                                   float energyPlayerSpeedMod,
                                   float enemyPointEatingSpeedMod,
                                   float enemyHomeBackSpeedMod,
                                   float redSpecialSpeedMod,
                                   float playerPointEatingSpeedMod,
                                   float enemyDispersionSpeedMod,
                                   float enemyTunnelSpeedMod,
                                   float enemyScaringSpeedMod,
                                   float playerSpeed,
                                   float enemySpeed,
                                   int enemySpeedBuffPointValue,
                                   int redSpeedBuffPointValue,
                                   int nextRoomExitPointValue,
                                   int maxDispersionCount,
                                   int playerLifeCount,
                                   float enemyScoreModifier,
                                   int fruitScore,
                                   int pointScore,
                                   int energyScore,
                                   int enemyScore)
            {
                FruitDestroyingTime = fruitDestroyingTime;
                EnergyBuffTime = energyBuffTime;
                EnemyDispersionTime = enemyDispersionTime;
                PlayerAFKTime = playerAFKTime;
                NextDispersionTime = nextDispersionTime;
                EnergyPlayerSpeedMod = energyPlayerSpeedMod;
                EnemyPointEatingSpeedMod = enemyPointEatingSpeedMod;
                EnemyHomeBackSpeedMod = enemyHomeBackSpeedMod;
                RedSpecialSpeedMod = redSpecialSpeedMod;
                PlayerPointEatingSpeedMod = playerPointEatingSpeedMod;
                EnemyDispersionSpeedMod = enemyDispersionSpeedMod;
                EnemyTunnelSpeedMod = enemyTunnelSpeedMod;
                EnemyScaringSpeedMod = enemyScaringSpeedMod;
                PlayerSpeed = playerSpeed;
                EnemySpeed = enemySpeed;
                EnemySpeedBuffPointValue = enemySpeedBuffPointValue;
                RedSpeedBuffPointValue = redSpeedBuffPointValue;
                NextRoomExitPointValue = nextRoomExitPointValue;
                MaxDispersionCount = maxDispersionCount;
                PlayerLifeCount = playerLifeCount;
                EnemyScoreModifier = enemyScoreModifier;
                FruitScore = fruitScore;
                PointScore = pointScore;
                EnergyScore = energyScore;
                EnemyScore = enemyScore;
            }
        }
        [Serializable]
        public enum LevelPartType
        {
            Empty,
            Wall,
            Point,
            Energy,
            Door,
            Fruit
        }

        public int LevelHeight;
        public int LevelWidth;

        public LevelPartType[/*columns*/][/*row*/] LevelMap;
        public Vector2Int PlayerSpawnPoint;
        public Vector2Int[] EnemySpawnPoints;
        public Vector2Int RoomPoint;
        public int[] FruitSpawnTriggers;
        public GlobalConstData GlobalConsts;

        public LevelData(int levelHeight,
                         int levelWidth,
                         LevelPartType[][] levelMap,
                         Vector2Int playerSpawnPoint,
                         Vector2Int[] enemySpawnPoints,
                         Vector2Int roomPoint,
                         int[] fruitSpawnTriggers,
                         GlobalConstData globalConsts)
        {
            LevelHeight = levelHeight;
            LevelWidth = levelWidth;
            LevelMap = levelMap;
            PlayerSpawnPoint = playerSpawnPoint;
            EnemySpawnPoints = enemySpawnPoints;
            RoomPoint = roomPoint;
            FruitSpawnTriggers = fruitSpawnTriggers;
            GlobalConsts = globalConsts;
        }

        public bool ValidateData()
        {
            if (LevelHeight <= 0)
                return false;
            if (LevelWidth <= 0)
                return false;
            if (LevelMap.Length != LevelHeight)
                return false;
            foreach (var column in LevelMap)
                if (column.Length != LevelWidth)
                    return false;

            return true;
        }
    }
}