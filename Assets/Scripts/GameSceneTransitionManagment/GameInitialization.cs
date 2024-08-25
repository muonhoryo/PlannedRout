

using System;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout
{
    public sealed class GameInitialization : MonoBehaviour
    {
        public static event Action GameInitializationEvent = delegate { };

        private void Start()
        {
#if UNITY_EDITOR
            var data = LevelSerialization.GetLevelDataFromFile(LevelManager.LevelEditorSerializationPath);
#else
            var data = LevelSerialization.GetLevelDataFromFile(LevelManager.LevelSerializationPath);
#endif
            LevelManager.Instance_.InitializeLevel(data);
            GameInitializationEvent();
        }
    }
}