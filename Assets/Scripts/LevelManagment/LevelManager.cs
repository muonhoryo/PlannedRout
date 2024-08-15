
using System;
using System.Linq;
using PlannedRout.LevelObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace PlannedRout.LevelManagment
{
    public sealed class LevelManager : MonoBehaviour
    {

        public const string LevelSerializationPath ="Level.json";
#if UNITY_EDITOR
        public const string LevelEditorSerializationPath ="Assets/Scripts/Editor/Level.json";
#endif

        public static LevelManager Instance_ { get; private set; }

        private LevelData? LvlData;
        public LevelData LevelData_ => (LevelData)LvlData;
        public LevelData.GlobalConstData GlobalConsts_ => LevelData_.GlobalConsts;

        private ILevelPart[/*columns*/][/*rows*/] LevelMap;

        private void Awake()
        {
            if (Instance_ != null)
                throw new System.Exception("There is more than one LevelManager.");

            Instance_ = this;
        }

        public void RegisterLevelData(LevelData data)
        {
            LevelMap=LevelLoader.LoadLevel(data);
            LvlData = data;
        }
        public void UnloadLevel()
        {
            LvlData = null;
            LevelMap = null;
        }
        public void AddLevelPart(ILevelPart part,int row,int column)
        {
            if (GetCell(row, column) != null)
                throw new System.Exception("Already have part at this cell.");

            LevelMap[column][row] = part;
        }
        public void RemoveLevelPart(int row,int column)
        {
            LevelMap[column][row].RemovePart();
            LevelMap[column][row] = null;
        }
        public ILevelPart GetCell(int row,int column)
        {
            return LevelMap[column][row];
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
                EnemiesPositions = new Vector2Int[]
                {
                    FindObjectPosition(ref EnemyTag_Red),
                    FindObjectPosition(ref EnemyTag_Pink),
                    FindObjectPosition(ref EnemyTag_Blue),
                    FindObjectPosition(ref EnemyTag_Orange)
                };
                DoorPosition = FindObjectPosition(ref DoorTag);

                LevelData.LevelPartType[][] levelMap = new LevelData.LevelPartType[LevelWidth][];
                for (int i = 0; i < LevelWidth; i++)
                    levelMap[i] = new LevelData.LevelPartType[LevelHeight];

                void PutPositionInMap(Vector2Int[] array, LevelData.LevelPartType type)
                {
                    foreach (var pos in array)
                        levelMap[pos.x][pos.y] = type;
                }

                levelMap[DoorPosition.x][FruitPosition.y] = LevelData.LevelPartType.Door;

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
                    enemySpawnPoints: EnemiesPositions,
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
            LevelManager.Instance_.RegisterLevelData(data);
        }

        private void OnValidate()
        {
            if (Instance_ == null)
                Instance_ = this;
        }
#endif
    }
}
