
using System;
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

        public LevelData LevelData_ { get; private set; }
        private ILevelPart[/*columns*/][/*rows*/] LevelMap;

        private void Awake()
        {
            if (Instance_ != null)
                throw new System.Exception("There is more than one LevelManager.");

            Instance_ = this;
        }

        public void RegisterLevelData(LevelData data)
        {
            if (!data.ValidateData())
                throw new Exception("Invalid data.");
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
    }
}
