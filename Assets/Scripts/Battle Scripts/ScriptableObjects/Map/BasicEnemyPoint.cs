
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Basic Enemy", menuName = "ScriptableObjects/Map/Points/Basic Enemy")]
    public class BasicEnemyPoint : NodePointAbstSO
    {
        public override NodeType PointType =>  NodeType.Basic_Enemy;

        public override void ActivatePoint()
        {
            OnEnterNode.PlaySound();
            var characterFactory = Factory.GameFactory.Instance.CharacterFactoryHandler;
           var enemySO= characterFactory.GetRandomCharacterSO(Battles.CharacterTypeEnum.Basic_Enemy);
           var enemy = characterFactory.CreateCharacter(enemySO);
            SinglePlayerHandler.Instance.RegisterOpponent(enemy);
            SinglePlayerHandler.Instance.Battle();
        }
    }
}