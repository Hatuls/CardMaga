
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Elite Enemy", menuName = "ScriptableObjects/Map/Points/Elite Enemy")]
    public class EliteEnemyPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>  EventPointType.Elite_Enemy;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}