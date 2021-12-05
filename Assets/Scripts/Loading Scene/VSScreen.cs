using UnityEngine;
using TMPro;
using System;

namespace UI.LoadingScreen
{
    public class VSScreen : AbstScreenTransition
    {
        [SerializeField]
        TextMeshProUGUI _playerNameText;
        [SerializeField]
        TextMeshProUGUI _opponentNameText;
        [SerializeField]
        GameObject _objectHolder;

        public override void StartTransition()
        {
            _animHash = Animator.StringToHash("PlayVSAnim");

            if (Battles.BattleData.Player == null)
            {
                throw new Exception("VSScreen BattleData player is null");
            }
            if (Battles.BattleData.Opponent == null)
            {
                throw new Exception("VSScreen BattleData opponent is null");
            }
            _playerNameText.text = Battles.BattleData.Player.CharacterData.Info.CharacterName;
            _opponentNameText.text = Battles.BattleData.Opponent.CharacterData.Info.CharacterName;
            StartAnimation();
        }

        void StartAnimation()
        {
            _objectHolder.SetActive(true);
            _screenAnimator.SetTrigger(_animHash);
        }
        public override void TransitionCompleted()
        {
            //can be problomatic
            _objectHolder.SetActive(false);
            base.TransitionCompleted();
        }
    }
}