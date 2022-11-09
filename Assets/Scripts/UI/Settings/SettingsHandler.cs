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

    public class CombatSettingsHandler : BaseUIElement
    {


        public void ToggleSFXSettings()
        {
            //  AudioManager.Instance.mu
        }
        public void ToggleMusicSettings()
        {

        }
        //     public void Open
    }


    public class SurrenderScreen : BaseUIElement, ISequenceOperation<IBattleUIManager>
    {
        public int Priority => 0;





        public void Close()
        {

        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager data)
        {
            throw new NotImplementedException();
        }
    }


    public abstract class BaseUIElement : MonoBehaviour, IUIElement
    {
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;
        [SerializeField, Tooltip("The GameObjects that will be turning on and off\nIf left empty it will close the gameobject this script is on")]
        private GameObject _gameObject;
        public void Hide()
        {
            OnHide?.Invoke();
            if (_gameObject != null)
                _gameObject.SetActive(false);
            else
                gameObject.SetActive(false);
        }

        public virtual void Init()
          => OnInitializable?.Invoke();


        public void Show()
        {
            OnShow?.Invoke();

            if (_gameObject != null)
                _gameObject.SetActive(true);
            else
                gameObject.SetActive(true);

        }
    }

}