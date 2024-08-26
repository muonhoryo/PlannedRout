

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
            public float PlayerAFKTime;
            public float PointEatingDebuffTime;
            //Speed buffs
            public float EnemyPointEatingSpeedMod;
            //Speed debuffs
            public float PlayerPointEatingSpeedMod;
            //Speed
            public float PlayerSpeed;
            public float EnemySpeed;
            //Point values
            public int EnemySpeedBuffPointValue;
            public int NextRoomExitPointValue;
            //Others
            public float MaxToTargetDistanceToChangeMovDirection;
            public float MinDistanceToPlayerDeath;
            //Given score
            public int PointScore;

            public GlobalConstData(
                                   float playerAFKTime,
                                   float pointEatingDebuffTime,
                                   float enemyPointEatingSpeedMod,
                                   float playerPointEatingSpeedMod,
                                   float playerSpeed,
                                   float enemySpeed,
                                   int enemySpeedBuffPointValue,
                                   int nextRoomExitPointValue,
                                   float maxToTargetDistanceToChangeMovDirection,
                                   float minDistanceToPlayerDeath,
                                   int pointScore)
            {
                PlayerAFKTime = playerAFKTime;
                PointEatingDebuffTime = pointEatingDebuffTime;
                EnemyPointEatingSpeedMod = enemyPointEatingSpeedMod;
                PlayerPointEatingSpeedMod = playerPointEatingSpeedMod;
                PlayerSpeed = playerSpeed;
                EnemySpeed = enemySpeed;
                EnemySpeedBuffPointValue = enemySpeedBuffPointValue;
                NextRoomExitPointValue = nextRoomExitPointValue;
                MaxToTargetDistanceToChangeMovDirection = maxToTargetDistanceToChangeMovDirection;
                MinDistanceToPlayerDeath = minDistanceToPlayerDeath;
                PointScore = pointScore;
            }
        }
        [Serializable]
        public struct LevelMap
        {
            public int Height;
            public int Width;
            [SerializeField]private LevelPartType[] Map;

            public LevelMap(int height, int width, LevelPartType[][] map)
            {
                if (height <= 0)
                    throw new Exception("Invalid level height.");
                if (width <= 0)
                    throw new Exception("Invalid level wdth.");

                if (map.Length != width)
                    throw new Exception("Invalid map data.");
                foreach (var column in map)
                    if (column.Length != height)
                        throw new Exception("Invalid map data.");

                Height = height;
                Width = width;
                Map = new LevelPartType[height * width];
                for(int i=0;i<width;i++)
                    for(int j = 0; j < height; j++)
                    {
                        Map[i * height + j] = map[i][j];
                    }
            }
            public LevelMap(int height,int width, LevelPartType[] map)
            {
                if (height <= 0)
                    throw new Exception("Invalid level height.");
                if (width <= 0)
                    throw new Exception("Invalid level wdth.");

                Height = height;
                Width = width;
                Map = map;
            }

            public LevelPartType GetLevelPart(int x, int y) =>
                Map[x * Height + y];
            public LevelPartType[][] GetMatrixMap()
            {
                LevelPartType[][] matrix = new LevelPartType[Width][];
                for (int i = 0; i < Width; i++)
                    matrix[i] = new LevelPartType[Height];

                for(int i= 0; i < Map.Length; i++)
                {
                    int x = i / Height;
                    int y=i % Height;
                    matrix[x][y] = Map[i];
                }
                return matrix;
            }
        }
        [Serializable]
        public enum LevelPartType
        {
            Empty,
            Wall,
            Point,
            Door
        }


        public LevelMap LvlMap;
        public Vector2Int PlayerSpawnPoint;
        public Vector2Int EnemySpawnPoint_Red;
        public Vector2Int EnemySpawnPoint_Blue;
        public Vector2Int EnemySpawnPoint_Pink;
        public Vector2Int EnemySpawnPoint_Orange;
        public GlobalConstData GlobalConsts;

        public LevelData(LevelMap levelMap,
                         Vector2Int playerSpawnPoint,
                         Vector2Int enemySpawnPoint_Red,
                         Vector2Int enemySpawnPoint_Blue,
                         Vector2Int enemySpawnPoint_Pink,
                         Vector2Int enemySpawnPoint_Orange,
                         GlobalConstData globalConsts)
        {
            LvlMap = levelMap;
            PlayerSpawnPoint = playerSpawnPoint;
            EnemySpawnPoint_Red = enemySpawnPoint_Red;
            EnemySpawnPoint_Blue= enemySpawnPoint_Blue;
            EnemySpawnPoint_Pink = enemySpawnPoint_Pink;
            EnemySpawnPoint_Orange= enemySpawnPoint_Orange;
            GlobalConsts = globalConsts;
        }
    }

    public struct LoadedLevelData
    {
        public ILevelPart[][] LevelMap;
        public GameObject PlayerCharacter;
        public GameObject EnemyCharacter_Red;
        public GameObject EnemyCharacter_Blue;
        public GameObject EnemyCharacter_Pink;
        public GameObject EnemyCharacter_Orange;
        public int OnLevelPointsCount;

        public LoadedLevelData(ILevelPart[][] levelMap, GameObject playerCharacter,GameObject enemyCharacter_red,
            GameObject enemyCharacter_blue,GameObject enemyCharacter_pink,GameObject enemyCharacter_orange,
            int onLevelPointsCount)
        {
            LevelMap = levelMap;
            PlayerCharacter = playerCharacter;
            EnemyCharacter_Red = enemyCharacter_red;
            EnemyCharacter_Blue = enemyCharacter_blue;
            EnemyCharacter_Pink = enemyCharacter_pink;
            EnemyCharacter_Orange = enemyCharacter_orange;
            OnLevelPointsCount = onLevelPointsCount;
        }
    }
}