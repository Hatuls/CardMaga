
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Boss Enemy", menuName = "ScriptableObjects/Map/Points/Boss Enemy")]
    public class BossEnemyPoint : NodePointAbstSO
    {
        public override NodeType PointType =>      NodeType.Boss_Enemy;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}