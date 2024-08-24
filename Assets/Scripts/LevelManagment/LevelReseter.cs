

using System;
using UnityEngine;

namespace PlannedRout.LevelManagment
{
    public static class LevelReseter
    {
        public static event Action LevelWasResetedEvent = delegate { };
        public static event Action LevelWasResetedPostEvent = delegate { };

        public static void Reset()
        {
            Vector2Int playerPos = LevelManager.Instance_.LevelData_.PlayerSpawnPoint;
            Vector2Int enemyPos_red = LevelManager.Instance_.LevelData_.EnemySpawnPoint_Red;
            Vector2Int enemyPos_blue = LevelManager.Instance_.LevelData_.EnemySpawnPoint_Blue ;
            Vector2Int enemyPos_pink = LevelManager.Instance_.LevelData_.EnemySpawnPoint_Pink ;
            Vector2Int enemyPos_orange=LevelManager.Instance_.LevelData_.EnemySpawnPoint_Orange;
            LevelManager.Instance_.PlayerCharacter_.transform.position = new Vector2(playerPos.x, playerPos.y);
            LevelManager.Instance_.EnemyCharacter_Red_.transform.position = new Vector2(enemyPos_red.x, enemyPos_red.y);
            LevelManager.Instance_.EnemyCharacter_Blue_.transform.position = new Vector2(enemyPos_blue.x, enemyPos_blue.y);
            LevelManager.Instance_.EnemyCharacter_Pink_.transform.position = new Vector2(enemyPos_pink.x, enemyPos_orange.y);
            LevelManager.Instance_.EnemyCharacter_Orange_.transform.position = new Vector2(enemyPos_orange.x, enemyPos_orange.y);
            LevelWasResetedEvent();
            LevelWasResetedPostEvent();
        }
    }
}