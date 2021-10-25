
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Basic Enemy", menuName = "ScriptableObjects/Map/Points/Basic Enemy")]
    public class BasicEnemyPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>  EventPointType.Basic_Enemy;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}