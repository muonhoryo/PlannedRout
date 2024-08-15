

using UnityEngine;

namespace PlannedRout.LevelObjects 
{
    [CreateAssetMenu]
    public sealed class LevelObjectsPrefabs : ScriptableObject
    {
        [SerializeField] private GameObject WallPrefab;
        [SerializeField] private GameObject PointPrefab;
        [SerializeField] private GameObject EnergyPrefab;
        [SerializeField] private GameObject FruitPrefab;
        [SerializeField] private GameObject DoorPrefab;
        [SerializeField] private GameObject RoomPointPrefab;
        [SerializeField] private GameObject PlayerPrefab;
        [SerializeField] private GameObject EnemyPrefab_Red;
        [SerializeField] private GameObject EnemyPrefab_Pink;
        [SerializeField] private GameObject EnemyPrefab_Blue;
        [SerializeField] private GameObject EnemyPrefab_Orange;

        public GameObject WallPrefab_ =>WallPrefab;
        public GameObject PointPrefab_ =>PointPrefab;
        public GameObject EnergyPrefab_ =>EnergyPrefab;
        public GameObject FruitPrefab_ =>FruitPrefab;
        public GameObject DoorPrefab_ =>DoorPrefab;
        public GameObject RoomPointPrefab_ =>RoomPointPrefab;
        public GameObject PlayerPrefab_ =>PlayerPrefab;
        public GameObject EnemyPrefab_Red_ =>EnemyPrefab_Red;
        public GameObject EnemyPrefab_Pink_ =>EnemyPrefab_Pink;
        public GameObject EnemyPrefab_Blue_ =>EnemyPrefab_Blue;
        public GameObject EnemyPrefab_Orange_ =>EnemyPrefab_Orange;
    }
}