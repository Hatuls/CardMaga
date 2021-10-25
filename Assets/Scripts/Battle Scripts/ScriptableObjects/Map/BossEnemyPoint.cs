
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Boss Enemy", menuName = "ScriptableObjects/Map/Points/Boss Enemy")]
    public class BossEnemyPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>      EventPointType.Boss_Enemy;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}