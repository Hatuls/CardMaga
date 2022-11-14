using Battle.Characters;
using Battle.Data;
using UnityEngine;

namespace CardMaga.UI.MatchMMaking
{
    public class MatchMakingUIManager : MonoBehaviour
    {
        [SerializeField] private TimeBasedOperation _matchMackingOperation;
        [SerializeField] private CharacterAssinger _mainCharacterAssginer;
        [SerializeField] private CharacterAssinger _opponentCharacterAssginer;
        
        [SerializeField] private GameObject _lookingForOpponentUI;
        [SerializeField] private GameObject _matchFoundUI;
        [Header("Tutorial")]
        [SerializeField] private float _delayVsScreen;

        private void Start()
        {
            Init(!(BattleData.Instance.BattleConfigSO.BattleTutorial == null));
        }

        private void Init(bool isInTutorial)
        {
            _mainCharacterAssginer.AssingCharecter(BattleData.Instance.Left);
            
            if (isInTutorial)
            {
            //    OpponentFound(BattleData.Instance.Right);
                _matchMackingOperation.DelayBeforeOperation = _delayVsScreen;
            }
            else
            {
               // MatchMakingManager.OnOpponentAssign += OpponentFound;
            }
        }

        private void OnDestroy()
        {
         //   MatchMakingManager.OnOpponentAssign -= OpponentFound;
        }

        //private void OpponentFound(BattleCharacter character)
        //{
        //    _lookingForOpponentUI.SetActive(false);
            
        //    _matchFoundUI.SetActive(true);
            
        //    _opponentCharacterAssginer.AssingCharecter(character);
        //}
    }
}

