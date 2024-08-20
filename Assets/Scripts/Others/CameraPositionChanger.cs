

using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout 
{
    public sealed class CameraPositionChanger : MonoBehaviour
    {
        private void Awake()
        {
            GameInitialization.GameInitializationEvent += InitializationComplete;
        }
        private void InitializationComplete()
        {
            GameInitialization.GameInitializationEvent -= InitializationComplete;
            transform.position = new Vector3((float)LevelManager.Instance_.LevelData_.LvlMap.Width / 2,
                (float)LevelManager.Instance_.LevelData_.LvlMap.Height / 2,
                transform.position.z);
        }
    }
}