
using System;
using System.Linq;
using PlannedRout.LevelObjects;
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
            LevelInitializedEvent();
        }
        public void UnloadLevel()
        {
            LvlData = null;
            LevelMap = null;
        }
        public void AddLevelPart(ILevelPart part,int column,int row)
        {
            if (GetCell(row, column) != null)
                throw new System.Exception("Already have part at this cell.");

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
            return row>=0&&row<LevelData_.LvlMap.Width
                && column>=0&&column<LevelData_.LvlMap.Height;
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
                Vector2Int FruitPosition;
                Vector2Int[] EnergyPositions;
                Vector2Int[] PointsPositions;
                Vector2Int[] WallsPositions;
                Vector2Int RoomPointPosition;
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
                FruitPosition = FindObjectPosition(ref FruitTag);
                EnergyPositions = FindObjectsPositions(ref EnergyTag);
                PointsPositions = FindObjectsPositions(ref PointTag);
                WallsPositions = FindObjectsPositions(ref WallTag);
                RoomPointPosition = FindObjectPosition(ref RoomPointTag);
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
                levelMap[FruitPosition.x][FruitPosition.y] = LevelData.LevelPartType.Fruit;

                PutPositionInMap(EnergyPositions, LevelData.LevelPartType.Energy);
                PutPositionInMap(PointsPositions, LevelData.LevelPartType.Point);
                PutPositionInMap(WallsPositions, LevelData.LevelPartType.Wall);

                int[] FruitSpawnTriggers;
                LevelData.GlobalConstData globalConsts;
                if (LvlData != null)
                {
                    FruitSpawnTriggers = LevelData_.FruitSpawnTriggers;
                    globalConsts = LevelData_.GlobalConsts;
                }
                else
                {
                    FruitSpawnTriggers = new int[0];
                    globalConsts = new LevelData.GlobalConstData();
                }

                LevelData.LevelMap compMap = new LevelData.LevelMap(LevelHeight, LevelWidth, levelMap);

                levelData = new
                    (levelMap: compMap,
                    playerSpawnPoint: PlayerPosition,
                    enemySpawnPoint_Red: EnemyPos_Red,
                    enemySpawnPoint_Blue: EnemyPos_Blue,
                    enemySpawnPoint_Pink: EnemyPos_Pink,
                    enemySpawnPoint_Orange: EnemyPos_Orange,
                    roomPoint: RoomPointPosition,
                    fruitSpawnPoint: FruitPosition,
                    fruitSpawnTriggers: FruitSpawnTriggers,
                    globalConsts: globalConsts);
            } //Level data generation

            LevelSerialization.GenerateLevelSave(levelData, LevelManager.LevelEditorSerializationPath);
        }

        [ContextMenu("LoadLevel")]
        public void LoadLevel()
        {
            var data = LevelSerialization.GetLevelDataFromFile(LevelManager.LevelEditorSerializationPath);
            LevelManager.Instance_.InitializeLevel(data);

            void PlaceObject(GameObject prefab,Vector2Int pos)
            {
                GameObject obj = GameObject.Instantiate(prefab, LevelLoadingData.Instance_.LevelParentObject.transform);
                obj.transform.position = (Vector2)pos;
            }
            PlaceObject(LevelLoadingData.Instance_.LevelObjsPrefabs.FruitPrefab_, data.FruitSpawnPoint);
            PlaceObject(LevelLoadingData.Instance_.LevelObjsPrefabs.RoomPointPrefab_, data.RoomPoint);
        }

        private void OnValidate()
        {
            if (Instance_ == null)
                Instance_ = this;
        }
#endif
    }
}
