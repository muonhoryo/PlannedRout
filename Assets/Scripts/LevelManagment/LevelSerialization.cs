

using System;
using System.IO;
using UnityEngine;

namespace PlannedRout.LevelManagment
{
    public static class LevelSerialization
    {
        public static string SerializeLevel(LevelData data)
        {
            return JsonUtility.ToJson(data, true);
        }
        public static void GenerateLevelSave(LevelData data,string filePath)
        {
            if (!File.Exists(filePath))
                File.Create(filePath);
            using( StreamWriter sw=new StreamWriter(filePath, false))
            {
                sw.Write(SerializeLevel(data));
            }
        }

        public static LevelData DeserializeLevel(string serializedData)
        {
            return JsonUtility.FromJson<LevelData>(serializedData);
        }
        public static LevelData GetLevelDataFromFile(string filePath)
        {
            using(StreamReader sr=new StreamReader(filePath))
            {
                return JsonUtility.FromJson<LevelData>(sr.ReadToEnd());
            }
        }
    }
}