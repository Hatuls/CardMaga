
using Battle.Characters;
using Battle.Combo;
using CardMaga.Card;
using Characters.Stats;
using System;
using UnityEngine;

namespace Managers
{
    public interface IPlayer
    {
        bool IsLeft { get; }
        CharacterStatsHandler StatsHandler { get; }
        CardData[] Deck { get; }
        Combo[] Combos { get; }
        VisualCharacter VisualCharacter { get; }
        void AssignCharacterData(Character characterData);
    }


    public class PlayerManager : MonoSingleton<PlayerManager>, IPlayer
    {
        #region Fields


        private Character _character;
        private CharacterStatsHandler _statsHandler;
        private CardData[] _playerDeck;
        [SerializeField] AnimatorController _playerAnimatorController;
        [SerializeField] AnimationBodyPartSoundsHandler _soundAnimation;
        #endregion
        static int Counter = 0;

        public CardData[] Deck => _playerDeck;
        public Combo[] Combos => _character.CharacterData.ComboRecipe;
        public bool IsLeft => true;
        public AnimatorController AnimatorController
        {
            get
            {
                if (_playerAnimatorController == null)
                {
                    var animators = FindObjectsOfType<AnimatorController>();

                    if (animators != null && animators.Length > 0)
                    {
                        foreach (var anim in animators)
                        {
                            if (anim.tag == "Player")
                            {
                                _playerAnimatorController = anim;
                                break;
                            }
                        }
                    }

                }
                return _playerAnimatorController;
            }

        }

        public CharacterStatsHandler StatsHandler { get => _statsHandler; }

        public VisualCharacter VisualCharacter => throw new NotImplementedException();

        public void AssignCharacterData(Character characterData)
        {


            //     Debug.LogWarning("<a>Spawning " + Counter++ + " </a>");
            _character = characterData;
            var data = characterData.CharacterData;
            _soundAnimation.CurrentCharacter = data.CharacterSO;

            int Length = data.CharacterDeck.Length;

            _playerDeck = new CardData[Length];
            Array.Copy(data.CharacterDeck, _playerDeck, Length);
            Battle.Deck.DeckManager.Instance.InitDeck(true, _playerDeck);
            _statsHandler = new CharacterStatsHandler(true, ref data.CharacterStats);

            //  CharacterStatsManager.RegisterCharacterStats(true, ref data.CharacterStats);
            AnimatorController.ResetAnimator();
        }


        public void OnEndTurn()
            => _playerAnimatorController.ResetLayerWeight();

        public void PlayerWin()
        {
            _playerAnimatorController.CharacterWon();
            _character.CharacterData.CharacterSO.VictorySound.PlaySound();
        }





    }

}
