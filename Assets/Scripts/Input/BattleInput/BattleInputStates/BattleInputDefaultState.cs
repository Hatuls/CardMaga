using System;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.Input
{
    public class BattleInputDefaultState : BaseState
    {
        [SerializeField] private HandUI _handUI;

        private IDisposable _handToken;
        
        public override void OnEnterState()
        {
            base.OnEnterState();

            _handToken = _handUI.HandLockTokenReceiver.GetToken();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            _handToken.Dispose();
        }
    }
}