using CardMaga.Battle;
using CardMaga.Battle.UI;
using CardMaga.Commands;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.UI.Settings
{

    public class SettingsHandler : MonoBehaviour, IUIElement
    {
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;

        public void Hide()
        {
            OnHide?.Invoke();
        }

        public void Init()
        {
            OnInitializable?.Invoke();
        }

        public void Show()
        {
            OnShow?.Invoke();
        }
    }


    public class SettingsLogic : MonoBehaviour, ISequenceOperation<IBattleUIManager>
    {
        public int Priority => 0;

        private IExecutableTask[] _executableTasks;

        [SerializeField]
        private GameObject _panel;
        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager data)
        {

            Init(data);
        }

        private void Init(IBattleUIManager battleUIManager)
        {
            var battleData = battleUIManager.BattleDataManager;
            _executableTasks = new IExecutableTask[]
            {
            new Surrender(battleData.EndBattleHandler),

            };

        }

        public void ExecuteLogic(int id)
            => _executableTasks[id].Execute();
    }


    public class Surrender : IExecutableTask
    {
        private readonly EndBattleHandler _endBattle;
        public Surrender(EndBattleHandler endBattle)
        {
            _endBattle = endBattle;
        }
        public void Execute()
        {
            bool isPlayer = true; // will need to change it to player lose not based 
            _endBattle.ForceEndBattle(!isPlayer);
        }
    }
    public class OpenSettings : IExecutableTask
    {
       // ViewWindowHandler
        public void Execute()
        {
            
        }
    }

   
}