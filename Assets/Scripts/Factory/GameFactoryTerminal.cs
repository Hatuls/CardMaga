using Battles;
using Collections.RelicsSO;
using UnityEngine;

namespace Factory
{
    public class GameFactoryTerminal : MonoBehaviour
    {
        [SerializeField] CardsCollectionSO cards;
        [SerializeField] ComboCollectionSO combos;
        [SerializeField] CharacterCollection characters;
        private void Awake()
        {

            if (cards == null)
            {
                cards = Resources.Load<CardsCollectionSO>("Collection SO/CardCollection");
                if (cards == null)
                    throw new System.Exception("Card Collection Was Not Assigned!");
            }

            if (combos == null)
            {
                combos = Resources.Load<ComboCollectionSO>("Collection SO/RecipeCollection");
                if (combos == null)
                    throw new System.Exception("Combo Collection Was Not Assigned!");
            }

            if (characters == null)
            {
                characters = Resources.Load<CharacterCollection>("Collection SO/CharacterCollection");
                if (characters == null)
                    throw new System.Exception("Characters Collection Was Not Assigned!");
            }


            GameFactory gameFactory = new GameFactory(cards, combos, characters);
            Destroy(this.gameObject);
        }
    }
     
}