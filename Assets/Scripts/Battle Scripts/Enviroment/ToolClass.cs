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
  
}