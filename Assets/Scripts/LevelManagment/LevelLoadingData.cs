

using PlannedRout.LevelObjects;
using UnityEngine;

namespace PlannedRout.LevelManagment 
{
    public sealed class LevelLoadingData : MonoBehaviour 
    {
        public static LevelLoadingData Instance_ { get; private set; }

        [SerializeField] public LevelObjectsPrefabs LevelObjsPrefabs;

        //xyzw, where is x - right, y - up, z - left, w - down
        //sprites assigned by 10-base represantation of his code

        [SerializeField] public Sprite[] WallSprites;
        [SerializeField] public GameObject LevelParentObject;

        private void Awake()
        {
#if UNITY_EDITOR
            if (Instance_ != null&&
                Instance_!=this)
                throw new System.Exception("Already have LevelLoadingData.");
#endif

            Instance_ = this;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if(Instance_==null)
                Instance_ = this;
        }
#endif
    }
}