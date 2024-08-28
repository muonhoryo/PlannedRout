

using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout 
{
    public sealed class CameraInitialization : MonoBehaviour
    {
        private void Awake()
        {
            GameInitialization.GameInitializationEvent += InitializationComplete;
        }
        private void InitializationComplete()
        {
            GameInitialization.GameInitializationEvent -= InitializationComplete;
            transform.position = new Vector3((float)(LevelManager.Instance_.LevelData_.LvlMap.Width-1) / 2,
                (float)(LevelManager.Instance_.LevelData_.LvlMap.Height-1) / 2 +LevelManager.Instance_.LevelData_.CameraLevelOffset,
                transform.position.z);
            Camera camComp=GetComponent<Camera>();
            camComp.orthographicSize=LevelManager.Instance_.LevelData_.CameraImageSize;
        }
    }
}