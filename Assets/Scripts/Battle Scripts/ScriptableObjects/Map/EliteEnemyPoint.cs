
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Elite Enemy", menuName = "ScriptableObjects/Map/Points/Elite Enemy")]
    public class EliteEnemyPoint : NodePointAbstSO
    {
        public override NodeType PointType =>  NodeType.Elite_Enemy;

        [Sirenix.OdinInspector.Button("Fight Elite Enemy")]
        public override void ActivatePoint()
        {
            var characterFactory = Factory.GameFactory.Instance.CharacterFactoryHandler;
            var enemySO = characterFactory.GetRandomCharacterSO(Battles.CharacterTypeEnum.Elite_Enemy);
            var enemy = characterFactory.CreateCharacter(enemySO);
            SinglePlayerHandler.Instance.RegisterOpponent(enemy);
            SinglePlayerHandler.Instance.Battle();
        }
    }
}