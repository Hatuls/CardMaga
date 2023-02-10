using Collections;
using ReiTools.TokenMachine;
using UnityEngine;

namespace Factory
{
    public class GameFactoryTerminal : MonoBehaviour
    {


        [SerializeField] Keywords.KeywordsCollectionSO _keywords;
        [SerializeField] CardsCollectionSO _cards;
        [SerializeField] ComboCollectionSO _combos;
        [SerializeField] CharacterCollectionSO _characters;
   //     [SerializeField] Rewards.BattleRewardCollectionSO _rewards;
        public static bool flag;

        public void Init(ITokenReceiver tokenReciever)
        {
            using (tokenReciever.GetToken())
            {

                if (!flag)
                {
                    flag = true;


                    if (_cards == null)
                    {
                        _cards = Resources.Load<CardsCollectionSO>("Collection SO/CardCollection");
                        if (_cards == null)
                            throw new System.Exception("BattleCard Collection Was Not Assigned!");
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

                    //if (_rewards == null)
                    //{
                    //    _rewards = Resources.Load<Rewards.BattleRewardCollectionSO>("Collection SO/BattleRewardsCollection");
                    //    if (_rewards == null)
                    //        throw new System.Exception("Reward Collection Was Not Assigned!");
                    //}


                    if (_keywords == null)
                    {
                        _keywords = Resources.Load<Keywords.KeywordsCollectionSO>("Collection SO/KeywordSOCollection");
                        if (_keywords == null)
                            throw new System.Exception("GameFactoryTerminal : KeywordCollection Was not assigned!");
                    }


                    new GameFactory(_cards, _combos, _characters,  _keywords);
                }

                Destroy(this.gameObject);
            }
        }
    }

}