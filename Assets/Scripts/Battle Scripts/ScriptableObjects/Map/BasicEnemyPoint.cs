
using UnityEngine;
using static CardMaga.ActDifficultySO;

namespace CardMaga
{
    [CreateAssetMenu(fileName = "Basic Enemy", menuName = "ScriptableObjects/Map/Points/Basic Enemy")]
    public class BasicEnemyPoint : NodePointAbstSO
    {
        public override NodeType PointType =>  NodeType.Basic_Enemy;
        public override void ActivatePoint(NodeLevel level)
        {
            var characterFactory = Factory.GameFactory.Instance.CharacterFactoryHandler;
           var enemySO= characterFactory.GetRandomCharacterSO(Battles.CharacterTypeEnum.Basic_Enemy, level);
           var enemy = characterFactory.CreateCharacter(enemySO);
            SinglePlayerHandler.Instance.RegisterOpponent(enemy);
            ActivatePoint();
        }
        public override void ActivatePoint()
        {
            OnEnterNode.PlaySound();
            SinglePlayerHandler.Instance.Battle();
        }
    }
}