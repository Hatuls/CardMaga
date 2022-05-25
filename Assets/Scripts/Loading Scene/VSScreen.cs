using UnityEngine;
using TMPro;
using System;

namespace UI.LoadingScene
{
    public class VSScreen : BlackScreenPanel
    {
    //    [SerializeField]
    //    TextMeshProUGUI _playerNameText;
    //    [SerializeField]
    //    TextMeshProUGUI _opponentNameText;

    //    public override void StartTransition()
    //    {
    //        _animHash = Animator.StringToHash("PlayVSAnim");
    //        var data = Account.AccountManager.Instance.BattleData;
    //        if (data.Player == null)
    //        {
    //            throw new Exception("VSScreen BattleData player is null");
    //        }
    //        if (data.Opponent == null)
    //        {
    //            throw new Exception("VSScreen BattleData opponent is null");
    //        }
    //        _playerNameText.text = data.Player.CharacterData.CharacterSO.CharacterName;
    //        _opponentNameText.text = data.Opponent.CharacterData.CharacterSO.CharacterName;
    //        StartAnimation();
    //    }

    //    void StartAnimation()
    //    {
    //        _objectHolder.SetActive(true);
    //        _screenAnimator.SetTrigger(_animHash);
    //    }
    //    public override void TransitionCompleted()
    //    {
    //        //can be problomatic
    //        _objectHolder.SetActive(false);
    //        base.TransitionCompleted();
    //    }
    }
}