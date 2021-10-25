using UnityEngine;

namespace Rei.Utilities
{
    public class ToolClass
    {
   

        // rotate to look Toward 
        public static Quaternion RotateToLookTowards(Transform rotateFrom, Transform RotateTo)
          => RotateToLookTowards(rotateFrom.position,RotateTo.position );

        public static Quaternion RotateToLookTowards(Vector3 rotateFrom, Vector3 RotateTo)
        => Quaternion.LookRotation(GetDirection(RotateTo, rotateFrom));

        // get random direction
        public static Vector3 GetDirection()
          => new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)).normalized;
        // get direction from 2 vectors
        public static Vector3 GetDirection(Vector3 myPos, Vector3 targetPos)
         => (targetPos - myPos).normalized;
    }
}


namespace Game.LoadAndSave
{
    public interface ISaveLoad
    {
        void SaveData();
        void LoadData();
    }
    public static class SaveSystem
    {
        public static void Save(string SaveReferenceName, string data)
        {
            if (SaveReferenceName == null || SaveReferenceName.Length == 0 || SaveReferenceName == "")
                throw new System.Exception($"SaveSystem: SaveReferenceName - {SaveReferenceName}\n is not valid!");
            else if (data == null || data.Length == 0 || data == "")
                throw new System.Exception($"SaveSystem: Data - {data}\n is not valid!");
            
            
            PlayerPrefs.SetString(SaveReferenceName, data);
        }

        public static string Load(string SaveReferenceName)
        {
            if (SaveReferenceName == null || SaveReferenceName.Length == 0 || SaveReferenceName == "")
                throw new System.Exception($"SaveSystem: String Parameter (SaveReferenceName - {SaveReferenceName}) is empty!");

            var data = PlayerPrefs.GetString(SaveReferenceName);

            if (data == null || data.Length == 0)
                throw new System.Exception($"SaveSystem: Could not load data from: <a>{SaveReferenceName}</a>!");

            return data;
        }
        
    }
}