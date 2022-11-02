using Battle;
using Battle.Turns;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
namespace CardMaga.Battle.UI
{

    public class EndTurnButton : ButtonUI, ISequenceOperation<IBattleUIManager>
    {
        public static event Action OnEndTurnButtonClicked;

        [SerializeField]
        private GameObject _visualizer;
        [SerializeField]
        SoundEventSO OnRejectSound;

        public int Priority => 0;
        private bool _isDirty;

        private void ShowTurn()
        {
            _visualizer.SetActive(true);
            _isDirty = false;
        }
        private void HideTurnButton() => _visualizer.SetActive(false);

        public override void ButtonPressed()
        {
            if (!_isDirty)
            {
                _isDirty = true;
                OnEndTurnButtonClicked?.Invoke();
            }

            //   OnRejectSound?.PlaySound();

        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager battleUIManager)
        {
            var data = battleUIManager.BattleDataManager;
            OnEndTurnButtonClicked += data.PlayersManager.GetCharacter(true).EndTurnHandler.EndTurnPressed;
            GameTurn left = data.TurnHandler.GetTurn(GameTurnType.LeftPlayerTurn);
            left.OnTurnActive += ShowTurn;
            left.OnTurnExit += HideTurnButton;
            data.OnBattleManagerDestroyed += BeforeDestroyed;
            BattleManager.OnGameEnded += HideTurnButton;
            HideTurnButton();
        }

        private void BeforeDestroyed(IBattleManager bm)
        {
            BattleManager.OnGameEnded -= HideTurnButton;
            var _turnHandler = bm.TurnHandler;
            OnEndTurnButtonClicked -= bm.PlayersManager.GetCharacter(true).EndTurnHandler.EndTurnPressed;
            bm.OnBattleManagerDestroyed -= BeforeDestroyed;
            var left = _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn);
            left.OnTurnActive -= ShowTurn;
            left.OnTurnExit -= HideTurnButton;
            _turnHandler = null;
        }
    }
}