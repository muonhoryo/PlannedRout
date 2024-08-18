

using System.Collections.Generic;
using PlannedRout.LevelObjects;
using UnityEngine;

namespace PlannedRout.LevelManagment
{
    public static class LevelLoader
    {
        public static LoadedLevelData LoadLevel(LevelData data)
        {
            ILevelPart[][] map =  new ILevelPart[data.LvlMap.Width][];
            for (int i = 0; i < data.LvlMap.Width; i++)
                map[i] = new ILevelPart[data.LvlMap.Width];

            List<Vector2Int> wallsPoses = new List<Vector2Int>();
            List<SpriteRenderer> wallsList = new List<SpriteRenderer>();

            void ChangeWallSprite(SpriteRenderer wall,Vector2Int wallPosition)
            {
                int wallcode = 15;
                void SetRightEmpty() => wallcode = wallcode & 14;
                void SetLeftEmpty() => wallcode = wallcode & 11;
                void SetTopEmpty() => wallcode = wallcode & 13;
                void SetBottomEmpty() => wallcode = wallcode & 7;
                bool IsThereWall(int x, int y) =>
                    map[x][y]!=null&&
                    (map[x][y].PartType_ == ILevelPart.LevelPartType.Wall||
                    map[x][y].PartType_==ILevelPart.LevelPartType.Door);

                if (wallPosition.x == data.LvlMap.Width - 1) //There is border right
                {
                    SetRightEmpty();
                    if (!IsThereWall(wallPosition.x-1,wallPosition.y)) //There is no wall left
                        SetLeftEmpty();
                }
                else
                {
                    if (!IsThereWall(wallPosition.x+1,wallPosition.y)) //There is no wall right
                        SetRightEmpty();
                    if (wallPosition.x == 0||
                        !IsThereWall(wallPosition.x-1,wallPosition.y)) //There is border or there is no wall left
                    {
                        SetLeftEmpty();
                    }
                }

                if(wallPosition.y==data.LvlMap.Height-1) //There is border top
                {
                    SetTopEmpty();
                    if (!IsThereWall(wallPosition.x, wallPosition.y - 1)) //There is no wall bottom
                        SetBottomEmpty();
                }
                else
                {
                    if (!IsThereWall(wallPosition.x, wallPosition.y + 1)) //There is no wall top
                        SetTopEmpty();
                    if(wallPosition.y==0||
                        !IsThereWall(wallPosition.x, wallPosition.y - 1)) //There is border o there is no wall bottom
                    {
                        SetBottomEmpty();
                    }
                }
                wall.sprite = LevelLoadingData.Instance_.WallSprites[wallcode];
            }

            Transform parentObj = LevelLoadingData.Instance_.LevelParentObject.transform;

            for (int x = 0; x < data.LvlMap.Width; x++)
                for(int y=0;y < data.LvlMap.Height; y++)
                {
                    GameObject InstantiateObj(GameObject prefab)
                    {
                        GameObject obj = GameObject.Instantiate(prefab, parentObj);
                        obj.transform.position = new Vector2(x, y);
                        return obj;
                    }

                    var part = data.LvlMap.GetLevelPart(x, y);
                    switch (part) 
                    {
                        case LevelData.LevelPartType.Wall:
                            {
                                GameObject wallObj = InstantiateObj(LevelLoadingData.Instance_.LevelObjsPrefabs.WallPrefab_);
                                wallsList.Add(wallObj.GetComponent<SpriteRenderer>());
                                wallsPoses.Add(new Vector2Int(x, y));
                                map[x][y] = Wall.Instance_;
                                break;
                            }
                        case LevelData.LevelPartType.Point:
                            {
                                GameObject pointObj = InstantiateObj(LevelLoadingData.Instance_.LevelObjsPrefabs.PointPrefab_);
                                map[x][y] = new Point(pointObj);
                                break;
                            }
                        case LevelData.LevelPartType.Energy:
                            {
                                GameObject energyObj = InstantiateObj(LevelLoadingData.Instance_.LevelObjsPrefabs.EnergyPrefab_);
                                map[x][y] = new Energy(energyObj);
                                break;
                            }
                        case LevelData.LevelPartType.Door:
                            {
                                InstantiateObj(LevelLoadingData.Instance_.LevelObjsPrefabs.DoorPrefab_);
                                map[x][y] = Door.Instance_;
                                break;
                            }
                    }
                } //Fill map with unmovable objects

            for (int i = 0; i < wallsList.Count; i++)
                ChangeWallSprite(wallsList[i], wallsPoses[i]);

            //Fill map with characters

            GameObject PlaceNoneMapObject(Vector2Int pos,GameObject prefab)
            {
                GameObject obj = GameObject.Instantiate(prefab, parentObj);
                obj.transform.position = (Vector2)pos;
                return obj;
            }

            GameObject playerObj = PlaceNoneMapObject(data.PlayerSpawnPoint, LevelLoadingData.Instance_.LevelObjsPrefabs.PlayerPrefab_);
            PlaceNoneMapObject(data.EnemySpawnPoint_Red, LevelLoadingData.Instance_.LevelObjsPrefabs.EnemyPrefab_Red_);
            PlaceNoneMapObject(data.EnemySpawnPoint_Blue, LevelLoadingData.Instance_.LevelObjsPrefabs.EnemyPrefab_Blue_);
            PlaceNoneMapObject(data.EnemySpawnPoint_Pink, LevelLoadingData.Instance_.LevelObjsPrefabs.EnemyPrefab_Pink_);
            PlaceNoneMapObject(data.EnemySpawnPoint_Orange, LevelLoadingData.Instance_.LevelObjsPrefabs.EnemyPrefab_Orange_);

            LoadedLevelData loadedData = new(map, playerObj);
            return loadedData;
        }
    }
}