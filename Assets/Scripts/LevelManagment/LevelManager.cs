
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using PlannedRout.LevelObjects;
using PlannedRout.LevelObjects.Characters;
using Unity.VisualScripting;
using UnityEngine;

namespace PlannedRout.LevelManagment
{
    public sealed class LevelManager : MonoBehaviour
    {
        public static event Action LevelInitializedEvent = delegate { };

        public const int EnemiesCount = 4;
        public const string LevelSerializationPath ="Level.json";
#if UNITY_EDITOR
        public const string LevelEditorSerializationPath ="Assets/Scripts/Editor/Level.json";
#endif

        public static LevelManager Instance_ { get; private set; }

        private LevelData? LvlData;
        public LevelData LevelData_ => (LevelData)LvlData;
        public LevelData.GlobalConstData GlobalConsts_ => LevelData_.GlobalConsts;

        public GameObject PlayerCharacter_ { get; private set; }
        public GameObject EnemyCharacter_Red_ { get; private set; }
        public GameObject EnemyCharacter_Blue_ { get; private set; }
        public GameObject EnemyCharacter_Pink_ { get; private set; }
        public GameObject EnemyCharacter_Orange_ { get; private set; }
        public int OnLevelPointCount_ { get; private set; }

        private ILevelPart[/*columns*/][/*rows*/] LevelMap;

        private void Awake()
        {
#if UNITY_EDITOR
            if (Instance_ != null &&
                Instance_!=this)
                throw new System.Exception("There is more than one LevelManager.");
#endif

            Instance_ = this;
        }

        public void InitializeLevel(LevelData data)
        {
            LvlData = data;
            var loadedData = LevelLoader.LoadLevel(data);
            LevelMap=loadedData.LevelMap;
            PlayerCharacter_=loadedData.PlayerCharacter;
            EnemyCharacter_Red_=loadedData.EnemyCharacter_Red;
            EnemyCharacter_Blue_ = loadedData.EnemyCharacter_Blue;
            EnemyCharacter_Pink_ = loadedData.EnemyCharacter_Pink;
            EnemyCharacter_Orange_ = loadedData.EnemyCharacter_Orange;
            OnLevelPointCount_ = loadedData.OnLevelPointsCount;
            LevelInitializedEvent();
        }
        public void UnloadLevel()
        {
            LvlData = null;
            LevelMap = null;
        }
        public void AddLevelPart(ILevelPart part,int column,int row)
        {
            if (GetCell(column, row) != null)
                throw new System.Exception("Already have part at this cell."+GetCell(column,row));

            LevelMap[column][row] = part;
        }
        public void RemoveLevelPart(int column,int row)
        {
            LevelMap[column][row].RemovePart();
            LevelMap[column][row] = null;
        }
        public ILevelPart GetCell(int column,int row)
        {
            return LevelMap[column][row];
        }
        public bool CheckCellPosition(int column,int row)
        {
            return row>=0&&row<LevelData_.LvlMap.Height
                && column>=0&&column<LevelData_.LvlMap.Width;
        }
        public Vector2Int GetNearestPassableCell(Vector2Int point,Vector2Int pathFindingStartPos,bool isCollideDoor=true)
        {
            List<Vector2Int> checkedCells = new List<Vector2Int> { point };
            Queue<Vector2Int> checkQueue = new Queue<Vector2Int>();
            checkQueue.Enqueue(point);

            while (checkQueue.Count!=0)
            {
                Vector2Int checkedCell=checkQueue.Dequeue();

                Vector2Int GetRightCell() =>
                    new Vector2Int(checkedCell.x + 1, checkedCell.y);
                Vector2Int GetLeftCell() =>
                    new Vector2Int(checkedCell.x - 1, checkedCell.y);
                Vector2Int GetTopCell() =>
                    new Vector2Int(checkedCell.x, checkedCell.y + 1);
                Vector2Int GetBottomCell() =>
                    new Vector2Int(checkedCell.x, checkedCell.y - 1);

                bool CheckCell()
                {
                    ILevelPart cell = GetCell(checkedCell.x, checkedCell.y);
                    if (cell != null &&
                        (cell.PartType_ == ILevelPart.LevelPartType.Wall ||
                        (isCollideDoor && cell.PartType_ == ILevelPart.LevelPartType.Door)))
                        return false;
                    else
                    {
                        var pathFinding = new PathFinding(pathFindingStartPos,checkedCell);
                        pathFinding.FindPath();
                        return pathFinding.Path != null;
                    }
                }

                if (!CheckCell())
                {
                    void QueueCell(Vector2Int addCell)
                    {
                        if (CheckCellPosition(addCell.x, addCell.y) &&
                             !checkedCells.Contains(addCell) &&
                             !checkQueue.Contains(addCell))
                        {
                            checkQueue.Enqueue(addCell);
                        }
                    }

                    QueueCell(GetRightCell());
                    QueueCell(GetLeftCell());
                    QueueCell(GetTopCell());
                    QueueCell(GetBottomCell());
                    checkedCells.Add(checkedCell);
                }
                else
                    return checkedCell;
            }
            Debug.LogError(point);
            throw new System.Exception("Invalid map or cannot finding free cell.");
        }
        public Vector2Int GetNearestPassableCell(Vector2Int point, bool isCollideDoor = true) =>
            GetNearestPassableCell(point, LevelData_.PlayerSpawnPoint, isCollideDoor);
        public Vector2Int GetFarthestCellAtDirection(Vector2Int origin,MovingComponent.MovingDirection direction,
            bool collideDoor=false)
        {
            Vector2Int NullResult()=>-Vector2Int.one;

            if (!CheckCellPosition(origin.x, origin.y))
                return NullResult();

            bool CheckCell(Vector2Int cellPos)
            {
                ILevelPart cell = GetCell(cellPos.x,cellPos.y);

                return cell == null ||
                    (cell.PartType_ != ILevelPart.LevelPartType.Wall &&
                    (!collideDoor || cell.PartType_ != ILevelPart.LevelPartType.Door));
            }

            Vector2Int GetRight()
            {
                Vector2Int pos;
                Vector2Int prevPos= NullResult();
                for(int i=origin.x+1;i<LevelData_.LvlMap.Width;i++) 
                {
                    pos = new Vector2Int(i, origin.y);
                    if (!CheckCell(pos))
                    {
                        if (i == origin.x + 1)
                            return NullResult();
                        else
                            return prevPos;
                    }
                    prevPos = pos;
                }
                return NullResult();
            }
            Vector2Int GetTop()
            {
                Vector2Int pos;
                Vector2Int prevPos = NullResult();
                for (int i = origin.y + 1; i < LevelData_.LvlMap.Height; i++)
                {
                    pos = new Vector2Int(origin.x, i);
                    if (!CheckCell(pos))
                    {
                        if (i == origin.y+1)
                            return NullResult();
                        else
                            return prevPos;
                    }
                    prevPos = pos;
                }
                return NullResult();
            }
            Vector2Int GetLeft()
            {
                Vector2Int pos;
                Vector2Int prevPos = NullResult();
                for (int i = origin.x - 1; i >=0; i--)
                {
                    pos = new Vector2Int(i, origin.y);
                    if (!CheckCell(pos))
                    {
                        if (i == origin.x-1)
                            return NullResult();
                        else
                            return prevPos;
                    }
                    prevPos = pos;
                }
                return NullResult();
            }
            Vector2Int GetBottom()
            {
                Vector2Int pos;
                Vector2Int prevPos = NullResult();
                for (int i = origin.y - 1; i >= 0; i--)
                {
                    pos = new Vector2Int(origin.x, i);
                    if (!CheckCell(pos))
                    {
                        if (i == origin.y-1)
                            return NullResult();
                        else
                            return prevPos;
                    }
                    prevPos = pos;
                }
                return NullResult();
            }

            switch (direction) 
            {
                case MovingComponent.MovingDirection.Right:
                    return GetRight();
                case MovingComponent.MovingDirection.Left:
                    return GetLeft();
                case MovingComponent.MovingDirection.Top:
                    return GetTop();
                case MovingComponent.MovingDirection.Bottom:
                    return GetBottom();
                default:
                    return -Vector2Int.one;
            }
        } 

        private void OnDestroy()
        {
            LevelManager.Instance_.UnloadLevel();
        }

        //Editor
#if UNITY_EDITOR
        [SerializeField] private int LevelHeight;
        [SerializeField] private int LevelWidth;
        [SerializeField] private string PlayerTag;
        [SerializeField] private string PointTag;
        [SerializeField] private string WallTag;
        [SerializeField] private string EnemyTag_Red;
        [SerializeField] private string EnemyTag_Blue;
        [SerializeField] private string EnemyTag_Pink;
        [SerializeField] private string EnemyTag_Orange;
        [SerializeField] private string DoorTag;

        [SerializeField] private LevelObjectsPrefabs Prefabs;

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
                Vector2Int[] PointsPositions;
                Vector2Int[] WallsPositions;
                Vector2Int EnemyPos_Red;
                Vector2Int EnemyPos_Blue;
                Vector2Int EnemyPos_Pink;
                Vector2Int EnemyPos_Orange;
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
                    GameObject foundObj = GameObject.FindGameObjectWithTag(tag);
                    if (foundObj == null)
                        return Vector2Int.zero;
                    Vector3 pos = foundObj.transform.position;
                    return new Vector2Int((int)pos.x, (int)pos.y);
                }

                PlayerPosition = FindObjectPosition(ref PlayerTag);
                PointsPositions = FindObjectsPositions(ref PointTag);
                WallsPositions = FindObjectsPositions(ref WallTag);
                EnemyPos_Red = FindObjectPosition(ref EnemyTag_Red);
                EnemyPos_Blue = FindObjectPosition(ref EnemyTag_Blue);
                EnemyPos_Pink = FindObjectPosition(ref EnemyTag_Pink);
                EnemyPos_Orange= FindObjectPosition(ref EnemyTag_Orange);
                DoorPosition = FindObjectPosition(ref DoorTag);

                LevelData.LevelPartType[][] levelMap = new LevelData.LevelPartType[LevelWidth][];
                for (int i = 0; i < LevelWidth; i++)
                    levelMap[i] = new LevelData.LevelPartType[LevelHeight];

                void PutPositionInMap(Vector2Int[] array, LevelData.LevelPartType type)
                {
                    foreach (var pos in array)
                        levelMap[pos.x][pos.y] = type;
                }

                levelMap[DoorPosition.x][DoorPosition.y] = LevelData.LevelPartType.Door;

                PutPositionInMap(PointsPositions, LevelData.LevelPartType.Point);
                PutPositionInMap(WallsPositions, LevelData.LevelPartType.Wall);

                bool isExistLvlData = LvlData != null;

                float cameraSize = isExistLvlData ? LevelData_.CameraImageSize : 1;
                float cameraLevelOffset = isExistLvlData ? LevelData_.CameraLevelOffset : 0;
                float guiSize = isExistLvlData ? LevelData_.GUISize : 1;

                LevelData.GlobalConstData globalConsts=isExistLvlData?LevelData_.GlobalConsts:new LevelData.GlobalConstData();

                LevelData.LevelMap compMap = new LevelData.LevelMap(LevelHeight, LevelWidth, levelMap);

                levelData = new
                    (levelMap: compMap,
                    playerSpawnPoint: PlayerPosition,
                    enemySpawnPoint_Red: EnemyPos_Red,
                    enemySpawnPoint_Blue: EnemyPos_Blue,
                    enemySpawnPoint_Pink: EnemyPos_Pink,
                    enemySpawnPoint_Orange: EnemyPos_Orange,
                    globalConsts: globalConsts,
                    cameraImageSize: cameraSize,
                    cameraLevelOffset: cameraLevelOffset,
                    guiSize: guiSize);
            } //Level data generation

            LevelSerialization.GenerateLevelSave(levelData, LevelManager.LevelEditorSerializationPath);
        }

        [ContextMenu("LoadLevel")]
        public void LoadLevel()
        {
            var data = LevelSerialization.GetLevelDataFromFile(LevelManager.LevelEditorSerializationPath);
            LevelManager.Instance_.InitializeLevel(data);
        }

        [ContextMenu("ResetLevel")]
        public void ResetLevel()
        {
            foreach (var ch in LevelLoadingData.Instance_.LevelParentObject.GetComponentsInChildren<Transform>(true))
                if(ch!=null&&ch.gameObject!=LevelLoadingData.Instance_.LevelParentObject)
                    DestroyImmediate(ch.gameObject);
        }

        private void OnValidate()
        {
            if (Instance_ == null)
                Instance_ = this;
        }
#endif
    }
}
