
using UnityEngine;
using static CardMaga.ActDifficultySO;

namespace CardMaga
{
    [CreateAssetMenu(fileName = "Boss Enemy", menuName = "ScriptableObjects/Map/Points/Boss Enemy")]
    public class BossEnemyPoint : NodePointAbstSO
    {
        [SerializeField] SceneIdentificationSO _sceneLoader;
        public override NodeType PointType =>      NodeType.Boss_Enemy;

        public override void ActivatePoint(NodeLevel level)
        {
            var characterFactory = Factory.GameFactory.Instance.CharacterFactoryHandler;
            var enemySO = characterFactory.GetRandomCharacterSO(Battle.CharacterTypeEnum.Boss_Enemy, level);
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