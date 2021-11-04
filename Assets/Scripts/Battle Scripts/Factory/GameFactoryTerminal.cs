using Battles;
using Collections;
using UnityEngine;
namespace Factory
{
    public class GameFactoryTerminal : MonoBehaviour
    {
        [SerializeField] CardsCollectionSO _cards;
        [SerializeField] ComboCollectionSO _combos;
        [SerializeField] CharacterCollectionSO _characters;
        [SerializeField] Rewards.BattleRewardCollectionSO _rewards;
        [SerializeField] EventPointCollectionSO _eventPoints;
        [SerializeField] Art.ArtSO _art;
        private void Awake()
        {
            if (GameFactory.Instance == null)
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
                    _eventPoints = Resources.Load<EventPointCollectionSO>("Collection SO/EventPointCollection");
                    if (_eventPoints == null)
                        throw new System.Exception("Event Point Collection Was Not Assigned!");
                }

                if (_art == null)
                {
                    _art = Resources.Load<Art.ArtSO>("Art/AllPalette/ART BLACKBOARD");
                    if (_art == null)
                        throw new System.Exception("ArtSO Was Not Assigned!");
                }

                GameFactory gameFactory = new GameFactory(_art, _cards, _combos, _characters, _rewards, _eventPoints);
            }
            Destroy(this.gameObject);
        }
    }

}