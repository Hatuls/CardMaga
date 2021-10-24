using Battles;
using Collections.RelicsSO;
using UnityEngine;

namespace Factory
{
    public class GameFactoryTerminal : MonoBehaviour
    {
        [SerializeField] CardsCollectionSO _cards;
        [SerializeField] ComboCollectionSO _combos;
        [SerializeField] CharacterCollectionSO _characters;
        [SerializeField] Rewards.BattleRewardCollectionSO _rewards;
        [SerializeField]
        Map.EventPointCollectionSO _eventPoints;
        private void Awake()
        {

            if (_cards == null)
            {
                _cards = Resources.Load<CardsCollectionSO>("Collection SO/CardCollection");
                if (_cards == null)
                    throw new System.Exception("Card Collection Was Not Assigned!");
            }

            if (_combos == null)
            {
                _combos = Resources.Load<ComboCollectionSO>("Collection SO/RecipeCollection");
                if (_combos == null)
                    throw new System.Exception("Combo Collection Was Not Assigned!");
            }

            if (_characters == null)
            {
                _characters = Resources.Load<CharacterCollectionSO>("Collection SO/CharacterCollection");
                if (_characters == null)
                    throw new System.Exception("Characters Collection Was Not Assigned!");
            }

            if (_rewards == null)
            {
                _rewards = Resources.Load<Rewards.BattleRewardCollectionSO>("Collection SO/BattleRewardsCollection");
                if (_rewards == null)
                    throw new System.Exception("Reward Collection Was Not Assigned!");
            }

            if (_eventPoints == null)
            {
                _eventPoints = Resources.Load<Map.EventPointCollectionSO>("Collection SO/EventPointCollection");
                if (_eventPoints == null)
                    throw new System.Exception("Event Point Collection Was Not Assigned!");
            }

            GameFactory gameFactory = new GameFactory(_cards, _combos, _characters, _rewards, _eventPoints);
            Destroy(this.gameObject);
        }
    }
     
}