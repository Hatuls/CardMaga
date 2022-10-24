using Battle.Characters;
using Battle.Data;
using UnityEngine;

namespace CardMaga.UI.MatchMMaking
{
    public class MatchMakingUIManager : MonoBehaviour
    {
        [SerializeField] private CharecterAssinger _mainCharacterAssginer;
        [SerializeField] private CharecterAssinger _opponentCharacterAssginer;
        
        [SerializeField] private GameObject _lookingForOpponentUI;
        [SerializeField] private GameObject _matchFoundUI;

        private void Start()
        {
            MatchMakingManager.OnOpponentAssign += OpponentFound;
            _mainCharacterAssginer.AssingCharecter(BattleData.Instance.Left);
        }

        private void OnDestroy()
        {
            MatchMakingManager.OnOpponentAssign -= OpponentFound;
        }

        public void OpponentFound(Character character)
        {
            _lookingForOpponentUI.SetActive(false);
            
            _matchFoundUI.SetActive(true);
            
            _opponentCharacterAssginer.AssingCharecter(character);
        }
    }
}

